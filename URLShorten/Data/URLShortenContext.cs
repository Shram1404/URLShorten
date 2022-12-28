using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using URLShorten.Models;

namespace URLShorten.Data
{
    public class URLShortenContext : DbContext
    {
        public URLShortenContext (DbContextOptions<URLShortenContext> options)
            : base(options)
        {
        }

        public DbSet<URLShorten.Models.URLShort> URLShort { get; set; } = default!;
    }
}
