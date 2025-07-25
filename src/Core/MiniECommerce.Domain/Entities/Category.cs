﻿namespace MiniECommerce.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public ICollection<Product> Products { get; set; } = new List<Product>();

    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}
