// <auto-generated>
//     This file was generated by a T4 template.
//     Don't change it directly as your change would get overwritten. Instead, make changes
//     to the .tt file (i.e. the T4 template) and save it to regenerate this file.
// </auto-generated>

using System;

namespace StackExchange.StacMan
{
    /// <summary>
    /// StacMan Comment, corresponding to Stack Exchange API v2's comment type
    /// http://api.stackexchange.com/docs/types/comment
    /// </summary>
    public partial class Comment : StacManType
    {
        /// <summary>
        /// body
        /// </summary>
        [Field("body")]
        public string Body { get; internal set; }

        /// <summary>
        /// body_markdown -- introduced in API version 2.1
        /// </summary>
        [Field("body_markdown")]
        public string BodyMarkdown { get; internal set; }

        /// <summary>
        /// comment_id
        /// </summary>
        [Field("comment_id")]
        public int CommentId { get; internal set; }

        /// <summary>
        /// creation_date
        /// </summary>
        [Field("creation_date")]
        public DateTime CreationDate { get; internal set; }

        /// <summary>
        /// edited
        /// </summary>
        [Field("edited")]
        public bool Edited { get; internal set; }

        /// <summary>
        /// link
        /// </summary>
        [Field("link")]
        public string Link { get; internal set; }

        /// <summary>
        /// owner
        /// </summary>
        [Field("owner")]
        public ShallowUser Owner { get; internal set; }

        /// <summary>
        /// post_id
        /// </summary>
        [Field("post_id")]
        public int PostId { get; internal set; }

        /// <summary>
        /// post_type
        /// </summary>
        [Field("post_type")]
        public Posts.PostType PostType { get; internal set; }

        /// <summary>
        /// reply_to_user
        /// </summary>
        [Field("reply_to_user")]
        public ShallowUser ReplyToUser { get; internal set; }

        /// <summary>
        /// score
        /// </summary>
        [Field("score")]
        public int Score { get; internal set; }

    }
}