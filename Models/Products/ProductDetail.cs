﻿namespace MediNet_BE.Models.Products
{
    public class ProductDetail
    {
        public int Id { get; set; }
		public string ImagesProductDetail { get; set; }
		public string Ingredient { get; set; }
        public string Usage { get; set; }
        public string UsageInstructions { get; set; }
        public string Description { get; set; }
        public string SideEffects { get; set; }
        public string Precautions { get; set; }
        public string Storage { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
