using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.IO;
using System.Text;

namespace LifeSpot;

public static class EndpointMapper
{
    /// <summary>
    ///  Маппинг CSS-файлов
    /// </summary>
    public static void MapCss(this IEndpointRouteBuilder builder)
    {
        var cssFiles = new[] { "index.css" };

        foreach (var fileName in cssFiles)
        {
            builder.MapGet($"/wwwroot/CSS/{fileName}", async context =>
            {
                var cssPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CSS", fileName);
                var css = await File.ReadAllTextAsync(cssPath);
                await context.Response.WriteAsync(css);
            });
        }
    }

    /// <summary>
    ///  Маппинг JS
    /// </summary>
    public static void MapJs(this IEndpointRouteBuilder builder)
    {
        var jsFiles = new[] { "index.js", "testing.js", "about.js" };

        foreach (var fileName in jsFiles)
        {
            builder.MapGet($"/wwwroot/JS/{fileName}", async context =>
            {
                var jsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "JS", fileName);
                var js = await File.ReadAllTextAsync(jsPath);
                await context.Response.WriteAsync(js);
            });
        }
    }

    /// <summary>
    ///  Маппинг img
    /// </summary>
    public static void MapImages(this IEndpointRouteBuilder builder)
    {
        var imageFiles = new[] { "london.jpg", "ny.jpg", "spb.jpg" };

        foreach (var fileName in imageFiles)
        {
            builder.MapGet($"/wwwroot/images/{fileName}", async context =>
            {
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                var img = await File.ReadAllBytesAsync(imgPath);
                var fileExtensions = Path.GetExtension(fileName).ToLower();

                var contentType = fileExtensions switch
                {
                    ".jpg" => "image/jpg",
                    ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    _ => "application/octet-stream"
                };

                await context.Response.Body.WriteAsync(img, 0, img.Length);
            });
        }
    }

    /// <summary>
    ///  Маппинг Html-страниц
    /// </summary>
    public static void MapHtml(this IEndpointRouteBuilder builder)
    {
        string footerHtml = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Views", "Shared", "footer.html"));
        string sideBarHtml = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Views", "Shared", "sidebar.html"));

        builder.MapGet("/", async context =>
        {
            var viewPath = Path.Combine(Directory.GetCurrentDirectory(), "Views", "index.html");
            var viewText = await File.ReadAllTextAsync(viewPath);

            // Загружаем шаблон страницы, вставляя в него элементы
            var html = new StringBuilder(await File.ReadAllTextAsync(viewPath))
                .Replace("<!--SIDEBAR-->", sideBarHtml)
                .Replace("<!--FOOTER-->", footerHtml);


            await context.Response.WriteAsync(html.ToString());
        });

        builder.MapGet("/testing", async context =>
        {
            var viewPath = Path.Combine(Directory.GetCurrentDirectory(), "Views", "testing.html");

            // Загружаем шаблон страницы, вставляя в него элементы
            var html = new StringBuilder(await File.ReadAllTextAsync(viewPath))
                .Replace("<!--SIDEBAR-->", sideBarHtml)
                .Replace("<!--FOOTER-->", footerHtml);

            await context.Response.WriteAsync(html.ToString());
        });

        builder.MapGet("/about", async context =>
        {
            var viewPath = Path.Combine(Directory.GetCurrentDirectory(), "Views", "about.html");
            var sliderHtml = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Views", "Shared", "slider.html"));

            // Загружаем шаблон страницы, вставляя в него элементы
            var html = new StringBuilder(await File.ReadAllTextAsync(viewPath))
                .Replace("<!--SIDEBAR-->", sideBarHtml)
                .Replace("<!--FOOTER-->", footerHtml)
                .Replace("<!--SLIDER-->", sliderHtml);

            await context.Response.WriteAsync(html.ToString());
        });
    }
}