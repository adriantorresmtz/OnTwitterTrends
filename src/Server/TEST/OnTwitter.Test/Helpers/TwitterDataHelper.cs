using OnTwitter.Domain.Entities;

namespace OnTwitter.Test.Helpers;

public static class TwitterDataHelper
{
    public static List<Twitter> GetTwitters_Fake() 
    {
        var twitters = new List<Twitter>() {
            new Twitter(){ Id = 1, TwitterId= "124568"},
            new Twitter(){ Id = 2, TwitterId= "958475"},
            new Twitter(){ Id = 3, TwitterId= "487512"},
            new Twitter(){ Id = 4, TwitterId= "658741"},
            new Twitter(){ Id = 5, TwitterId= "772230"},
        };

        return twitters;
    }

    public static List<TwitterHashTag> GetTwitterHashTags_Fake()
    {
        var twitterHashTags = new List<TwitterHashTag>() {
            new TwitterHashTag(){ Id = 1, TwitterId= "124568", HashTag="Developer"},
            new TwitterHashTag(){ Id = 2, TwitterId= "958475", HashTag="Developer"},
            new TwitterHashTag(){ Id = 3, TwitterId= "487512", HashTag="Developer"},
            new TwitterHashTag(){ Id = 4, TwitterId= "658741", HashTag="Tester"},
            new TwitterHashTag(){ Id = 5, TwitterId= "772230", HashTag="Tester"},
        };

        return twitterHashTags;
    }
}
