﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace neu.Models
{
    public class BangumiEntry : INeuContent
    {
        // Bangumi subject_id
        public int Id { get; set; }

        // Collection status: wish/collect/do/on_hold/dropped
        public string Status { get; set; }

        // Comment
        public string Content { get; set; }

        // Subject tags
        public IEnumerable<string> Tags { get; set; }

        // Rating: 0 ~ 10
        public int Rating { get; set; }

        // Privacy: 0 - public, 1 - private
        public int Privacy { get; set; }

        // Date of adding subject to collection
        public DateTime CreateDate { get; set; }
    }
}
