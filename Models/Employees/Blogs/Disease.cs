﻿namespace MediNet_BE.Models.Employees.Blogs
{
    public class Disease
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Blog>? Blogs { get; set; }
    }
}
