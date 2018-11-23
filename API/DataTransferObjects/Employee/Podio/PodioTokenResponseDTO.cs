namespace DataTransferObjects.Employee.Podio
{
    public class PodioTokenResponseDTO
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public Ref _ref { get; set; }
        public string refresh_token { get; set; }

        public class Ref
        {
            public string type { get; set; }
            public int id { get; set; }
        }

    }
}
