using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Employee.Podio
{
    public class PodioEmployeeResponseDTO
    {
        public int filtered { get; set; }
        public int total { get; set; }
        public Item[] items { get; set; }

        public class Item
        {
            public Ratings ratings { get; set; }
            public object sharefile_vault_url { get; set; }
            public string last_event_on { get; set; }
            public string[] rights { get; set; }
            public int app_item_id { get; set; }
            public Field[] fields { get; set; }
            public string title { get; set; }
            public Initial_Revision initial_revision { get; set; }
            public Created_Via1 created_via { get; set; }
            public Created_By1 created_by { get; set; }
            public Current_Revision current_revision { get; set; }
            public int priority { get; set; }
            public int comment_count { get; set; }
            public int file_count { get; set; }
            public string created_on { get; set; }
            public int item_id { get; set; }
            public string link { get; set; }
            public object sharefile_vault_folder_id { get; set; }
            public int revision { get; set; }
            public object external_id { get; set; }
            public string app_item_id_formatted { get; set; }
        }

        public class Ratings
        {
            public Like like { get; set; }
        }

        public class Like
        {
            public object average { get; set; }
            public Counts counts { get; set; }
        }

        public class Counts
        {
            public _1 _1 { get; set; }
        }

        public class _1
        {
            public int total { get; set; }
            public User[] users { get; set; }
        }

        public class User
        {
            public int user_id { get; set; }
            public string name { get; set; }
            public object space_id { get; set; }
            public int profile_id { get; set; }
            public object org_id { get; set; }
            public string last_seen_on { get; set; }
            public string link { get; set; }
            public int avatar { get; set; }
            public string type { get; set; }
            public Image image { get; set; }
        }

        public class Image
        {
            public string hosted_by { get; set; }
            public string hosted_by_humanized_name { get; set; }
            public string thumbnail_link { get; set; }
            public string link { get; set; }
            public int file_id { get; set; }
            public object external_file_id { get; set; }
            public string link_target { get; set; }
        }

        public class Initial_Revision
        {
            public long item_revision_id { get; set; }
            public Created_Via created_via { get; set; }
            public Created_By created_by { get; set; }
            public string created_on { get; set; }
            public User1 user { get; set; }
            public string type { get; set; }
            public int revision { get; set; }
        }

        public class Created_Via
        {
            public object url { get; set; }
            public int auth_client_id { get; set; }
            public bool display { get; set; }
            public string name { get; set; }
            public int id { get; set; }
        }

        public class Created_By
        {
            public int user_id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public string type { get; set; }
            public Image1 image { get; set; }
            public string avatar_type { get; set; }
            public int? avatar { get; set; }
            public int id { get; set; }
            public int? avatar_id { get; set; }
            public string last_seen_on { get; set; }
        }

        public class Image1
        {
            public string hosted_by { get; set; }
            public string hosted_by_humanized_name { get; set; }
            public string thumbnail_link { get; set; }
            public string link { get; set; }
            public int file_id { get; set; }
            public object external_file_id { get; set; }
            public string link_target { get; set; }
        }

        public class User1
        {
            public int user_id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public string type { get; set; }
            public Image2 image { get; set; }
            public string avatar_type { get; set; }
            public int? avatar { get; set; }
            public int id { get; set; }
            public int? avatar_id { get; set; }
            public string last_seen_on { get; set; }
        }

        public class Image2
        {
            public string hosted_by { get; set; }
            public string hosted_by_humanized_name { get; set; }
            public string thumbnail_link { get; set; }
            public string link { get; set; }
            public int file_id { get; set; }
            public object external_file_id { get; set; }
            public string link_target { get; set; }
        }

        public class Created_Via1
        {
            public object url { get; set; }
            public int auth_client_id { get; set; }
            public bool display { get; set; }
            public string name { get; set; }
            public int id { get; set; }
        }

        public class Created_By1
        {
            public int user_id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public string type { get; set; }
            public Image3 image { get; set; }
            public string avatar_type { get; set; }
            public int? avatar { get; set; }
            public int id { get; set; }
            public int? avatar_id { get; set; }
            public string last_seen_on { get; set; }
        }

        public class Image3
        {
            public string hosted_by { get; set; }
            public string hosted_by_humanized_name { get; set; }
            public string thumbnail_link { get; set; }
            public string link { get; set; }
            public int file_id { get; set; }
            public object external_file_id { get; set; }
            public string link_target { get; set; }
        }

        public class Current_Revision
        {
            public long item_revision_id { get; set; }
            public Created_Via2 created_via { get; set; }
            public Created_By2 created_by { get; set; }
            public string created_on { get; set; }
            public User2 user { get; set; }
            public string type { get; set; }
            public int revision { get; set; }
        }

        public class Created_Via2
        {
            public string url { get; set; }
            public int auth_client_id { get; set; }
            public bool display { get; set; }
            public string name { get; set; }
            public int id { get; set; }
        }

        public class Created_By2
        {
            public int user_id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public string type { get; set; }
            public Image4 image { get; set; }
            public string avatar_type { get; set; }
            public int avatar { get; set; }
            public int id { get; set; }
            public int avatar_id { get; set; }
            public string last_seen_on { get; set; }
        }

        public class Image4
        {
            public string hosted_by { get; set; }
            public string hosted_by_humanized_name { get; set; }
            public string thumbnail_link { get; set; }
            public string link { get; set; }
            public int file_id { get; set; }
            public object external_file_id { get; set; }
            public string link_target { get; set; }
        }

        public class User2
        {
            public int user_id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
            public string type { get; set; }
            public Image5 image { get; set; }
            public string avatar_type { get; set; }
            public int avatar { get; set; }
            public int id { get; set; }
            public int avatar_id { get; set; }
            public string last_seen_on { get; set; }
        }

        public class Image5
        {
            public string hosted_by { get; set; }
            public string hosted_by_humanized_name { get; set; }
            public string thumbnail_link { get; set; }
            public string link { get; set; }
            public int file_id { get; set; }
            public object external_file_id { get; set; }
            public string link_target { get; set; }
        }

        public class Field
        {
            public string type { get; set; }
            public int field_id { get; set; }
            public string label { get; set; }
            public Value[] values { get; set; }
            public Config config { get; set; }
            public string external_id { get; set; }
        }

        public class Config
        {
            public Settings settings { get; set; }
            public object mapping { get; set; }
            public string label { get; set; }
        }

        public class Settings
        {
            public string format { get; set; }
            public string size { get; set; }
            public bool multiple { get; set; }
            public Option[] options { get; set; }
            public string display { get; set; }
            public string[] allowed_mimetypes { get; set; }
        }

        public class Option
        {
            public string status { get; set; }
            public string text { get; set; }
            public int id { get; set; }
            public string color { get; set; }
        }

        public class Value
        {
            public object value { get; set; }
        }

    }
}
