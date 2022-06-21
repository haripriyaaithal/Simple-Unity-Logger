namespace SimpleLogger.IO
{
    [System.Serializable]
    public class GistsResponseObject
    {
        public string url;
        public string forks_url;
        public string commits_url;
        public string id;
        public string node_id;
        public string git_pull_url;
        public string git_push_url;
        public string html_url;
        public string created_at;
        public string updated_at;
        public string description;
        public int comments;
        public object user;
        public string comments_url;
        public Owner owner;
        public bool truncated;
    }

    [System.Serializable]
    public class Owner
    {
        public string login;
        public int id;
        public string node_id;
        public string avatar_url;
        public string gravatar_id;
        public string url;
        public string html_url;
        public string followers_url;
        public string following_url;
        public string gists_url;
        public string starred_url;
        public string subscriptions_url;
        public string organizations_url;
        public string repos_url;
        public string events_url;
        public string received_events_url;
        public string type;
        public bool site_admin;
    }

    [System.Serializable]
    public class History
    {
        public User user;
        public string version;
        public string committed_at;
        public ChangeStatus change_status;
        public string url;
    }

    [System.Serializable]
    public class User
    {
        public string login;
        public int id;
        public string node_id;
        public string avatar_url;
        public string gravatar_id;
        public string url;
        public string html_url;
        public string followers_url;
        public string following_url;
        public string gists_url;
        public string starred_url;
        public string subscriptions_url;
        public string organizations_url;
        public string repos_url;
        public string events_url;
        public string received_events_url;
        public string type;
        public bool site_admin;
    }

    [System.Serializable]
    public class ChangeStatus
    {
        public int total;
        public int additions;
        public int deletions;
    }
}