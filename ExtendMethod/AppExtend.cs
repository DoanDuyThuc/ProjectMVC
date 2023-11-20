using System.Net;

namespace ProjectMVC.ExtendMethod
{
    public static class AppExtend
    {
        public static void AddStatusCodePage(this IApplicationBuilder app){
            app.UseStatusCodePages(appError => {
            appError.Run(async context => {
                var respone = context.Response;
                var code = respone.StatusCode;

                var content = @$"<html>
                    <head>
                        <meta charset='UTF-8' />
                        <title>Lỗi {code}</title>
                        <body>
                            <p>
                                có lỗi xảy ra {code} - {(HttpStatusCode)code}
                            </p>
                        </body>
                    </head>
                </html>";

                await respone.WriteAsync(content);
            });
        });
        }
        
    }
    
}