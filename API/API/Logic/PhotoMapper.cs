﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Data.Models;

namespace API.Logic
{
    public class PhotoMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64EncodedPhoto"></param>
        /// <param name="organization"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">If the encoded string does not contain a media type.</exception>
        public Photo ParseBase64Photo(string base64EncodedPhoto, Organization organization)
        {
            const string pattern = @"^data:([a-z/]+);base64,";
            var match = Regex.Match(base64EncodedPhoto, pattern);

            if (!match.Success)
            {
                throw new ArgumentOutOfRangeException(nameof(base64EncodedPhoto), "Unsupported image type");
            }

            var type = match.Groups[1].Value;

            var data = Convert.FromBase64String(Regex.Replace(base64EncodedPhoto, pattern, ""));

            return new Photo
            {
                Data = data,
                Type = type,
                Organization = organization
            };
        }
    }
}