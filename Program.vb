Imports Microsoft.AspNetCore.Builder
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting

Namespace QBIntegration
    Module Program
        Sub Main(args As String())
            Dim builder = WebApplication.CreateBuilder(args)

            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation().
                AddRazorOptions(Sub(options)
                                    options.ViewLocationFormats.Add("/Views/{1}/{0}" & ".vbhtml")
                                    options.ViewLocationFormats.Add("/Views/Shared/{0}" & ".vbhtml")
                                End Sub)

            Dim app = builder.Build()

            ' Configure the HTTP request pipeline.
            If app.Environment.IsDevelopment() Then
                app.UseDeveloperExceptionPage()
            Else
                app.UseExceptionHandler("/Home/Error")
                ' The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts()
            End If

            app.UseHttpsRedirection()
            app.UseStaticFiles()

            app.UseRouting()

            app.UseAuthorization()

            app.MapControllerRoute(
            name:="default",
            pattern:="{controller=QuickBooks}/{action=Index}/{id?}")

            app.Run()
        End Sub
    End Module
End Namespace
