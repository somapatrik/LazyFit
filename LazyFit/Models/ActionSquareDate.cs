﻿
namespace LazyFit.Models
{
    public class ActionSquareDate
    {
        public DateTime DateTime { get; set; }
        public List<ActionSquare> Actions { get; set; }

        public ActionSquareDate()
        {
            Actions = new List<ActionSquare>();
        }

    }
}