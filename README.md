# Recipes - A web site backed by github markdowns

This is a site that optimized for low costs and easy backend handling.  
- Low costs:   
   it is a [Blazor WebAssembly](https://devblogs.microsoft.com/aspnet/blazor-webassembly-3-2-0-now-available/) and being served as a [static site over azure storage](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-static-website)

- Easy backend handling:  
   There is no data base. The recipes are stored and served from [issues labled as recipe](https://github.com/turner11/Recipes/issues?q=label%3Arecipe+) in this repo.  
   This also enables some meta information using the issue labels.  
   Any issue formatted as "X: Y" will be classifed as _In Category X - has Attribute Y_ e.g. "diffculty: easy"
   
 The limitation of using github as a backend is that we are limited for 5k requests per hour. But since this is for personal use - I'm ok with that.  
 
 
 Take a look at [the site!](https://recipessite.z20.web.core.windows.net/)
 
 ---
 
 
![site example image](site_image.png)
