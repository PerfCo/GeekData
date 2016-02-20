// <auto-generated>
//     This file was generated by a T4 template.
//     Don't change it directly as your change would get overwritten. Instead, make changes
//     to the .tt file (i.e. the T4 template) and save it to regenerate this file.
// </auto-generated>

using System;

namespace StackExchange.StacMan
{
    /// <summary>
    /// StacMan Question, corresponding to Stack Exchange API v2's question type
    /// http://api.stackexchange.com/docs/types/question
    /// </summary>
    public partial class Question : StacManType
    {
        /// <summary>
        /// accepted_answer_id
        /// </summary>
        [Field("accepted_answer_id")]
        public int? AcceptedAnswerId { get; internal set; }

        /// <summary>
        /// answer_count
        /// </summary>
        [Field("answer_count")]
        public int AnswerCount { get; internal set; }

        /// <summary>
        /// answers
        /// </summary>
        [Field("answers")]
        public Answer[] Answers { get; internal set; }

        /// <summary>
        /// body
        /// </summary>
        [Field("body")]
        public string Body { get; internal set; }

        /// <summary>
        /// bounty_amount
        /// </summary>
        [Field("bounty_amount")]
        public int? BountyAmount { get; internal set; }

        /// <summary>
        /// bounty_closes_date
        /// </summary>
        [Field("bounty_closes_date")]
        public DateTime? BountyClosesDate { get; internal set; }

        /// <summary>
        /// close_vote_count -- introduced in API version 2.1
        /// </summary>
        [Field("close_vote_count")]
        public int CloseVoteCount { get; internal set; }

        /// <summary>
        /// closed_date
        /// </summary>
        [Field("closed_date")]
        public DateTime? ClosedDate { get; internal set; }

        /// <summary>
        /// closed_reason
        /// </summary>
        [Field("closed_reason")]
        public string ClosedReason { get; internal set; }

        /// <summary>
        /// comments
        /// </summary>
        [Field("comments")]
        public Comment[] Comments { get; internal set; }

        /// <summary>
        /// community_owned_date
        /// </summary>
        [Field("community_owned_date")]
        public DateTime? CommunityOwnedDate { get; internal set; }

        /// <summary>
        /// creation_date
        /// </summary>
        [Field("creation_date")]
        public DateTime CreationDate { get; internal set; }

        /// <summary>
        /// delete_vote_count -- introduced in API version 2.1
        /// </summary>
        [Field("delete_vote_count")]
        public int DeleteVoteCount { get; internal set; }

        /// <summary>
        /// down_vote_count
        /// </summary>
        [Field("down_vote_count")]
        public int DownVoteCount { get; internal set; }

        /// <summary>
        /// favorite_count
        /// </summary>
        [Field("favorite_count")]
        public int FavoriteCount { get; internal set; }

        /// <summary>
        /// is_answered
        /// </summary>
        [Field("is_answered")]
        public bool IsAnswered { get; internal set; }

        /// <summary>
        /// last_activity_date
        /// </summary>
        [Field("last_activity_date")]
        public DateTime LastActivityDate { get; internal set; }

        /// <summary>
        /// last_edit_date
        /// </summary>
        [Field("last_edit_date")]
        public DateTime? LastEditDate { get; internal set; }

        /// <summary>
        /// link
        /// </summary>
        [Field("link")]
        public string Link { get; internal set; }

        /// <summary>
        /// locked_date
        /// </summary>
        [Field("locked_date")]
        public DateTime? LockedDate { get; internal set; }

        /// <summary>
        /// migrated_from
        /// </summary>
        [Field("migrated_from")]
        public MigrationInfo MigratedFrom { get; internal set; }

        /// <summary>
        /// migrated_to
        /// </summary>
        [Field("migrated_to")]
        public MigrationInfo MigratedTo { get; internal set; }

        /// <summary>
        /// notice -- introduced in API version 2.1
        /// </summary>
        [Field("notice")]
        public Notice Notice { get; internal set; }

        /// <summary>
        /// owner
        /// </summary>
        [Field("owner")]
        public ShallowUser Owner { get; internal set; }

        /// <summary>
        /// protected_date
        /// </summary>
        [Field("protected_date")]
        public DateTime? ProtectedDate { get; internal set; }

        /// <summary>
        /// question_id
        /// </summary>
        [Field("question_id")]
        public int QuestionId { get; internal set; }

        /// <summary>
        /// reopen_vote_count -- introduced in API version 2.1
        /// </summary>
        [Field("reopen_vote_count")]
        public int ReopenVoteCount { get; internal set; }

        /// <summary>
        /// score
        /// </summary>
        [Field("score")]
        public int Score { get; internal set; }

        /// <summary>
        /// tags
        /// </summary>
        [Field("tags")]
        public string[] Tags { get; internal set; }

        /// <summary>
        /// title
        /// </summary>
        [Field("title")]
        public string Title { get; internal set; }

        /// <summary>
        /// up_vote_count
        /// </summary>
        [Field("up_vote_count")]
        public int UpVoteCount { get; internal set; }

        /// <summary>
        /// view_count
        /// </summary>
        [Field("view_count")]
        public int ViewCount { get; internal set; }

    }
}