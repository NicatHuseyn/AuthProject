﻿using System.ComponentModel.DataAnnotations.Schema;

namespace AuthProject.Core.Entities;

public class Product:BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }

    
    public string AppUserId { get; set; }
}
