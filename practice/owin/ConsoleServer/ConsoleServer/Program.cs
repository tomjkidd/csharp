using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Owin;
/* The Owin namespace defines IAppBuilder */

using Microsoft.Owin.Hosting;
/* The Microsoft.Owin.Hosting namespace provides StartOptions and WebApp */

using Microsoft.Owin;
/* The Microsoft.Owin namespace provides IOwinContext, OwinContext */

namespace ConsoleServer {
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using System.IO;

    class Program {
        static void Main(string[] args) {
            WebApp.Start<Startup>("http://localhost:8080");
            Console.WriteLine("Server started; Press enter to quit");
            Console.ReadLine();
        }
    }

    public class Startup { 
        public void Configuration(IAppBuilder app) {
            var middleware1 = new Func<AppFunc, AppFunc>(Middleware1);
            var middleware2 = new Func<AppFunc, AppFunc>(Middleware2);
            var middleware3 = new Func<AppFunc, AppFunc>(Middleware3);

            app.Use(middleware1);
            app.Use(middleware2);
            app.Use(middleware3);

            app.Use<Middleware4>(); // via class
            app.UseMiddleware4(); // via Extension method
            app.UseMiddleware5("Middleware5"); // via Extension with parameter

            var options = new MiddlewareConfigOptions("Hello", "Tom");
            options.IncludeDate = true;
            app.UseMiddleware6(options); // via extension with options
        }

        public AppFunc Middleware1(AppFunc next) {
            AppFunc appFunc = async (IDictionary<string, object> env) => {
                var response = env["owin.ResponseBody"] as Stream;
                using (var writer = new StreamWriter(response)) {
                    await writer.WriteAsync("<h1>Middleware1</h1>");
                }

                await next.Invoke(env);
            };
            return appFunc;
        }

        public AppFunc Middleware2(AppFunc next) {
            AppFunc appFunc = async (IDictionary<string, object> env) => {
                var response = env["owin.ResponseBody"] as Stream;
                using (var writer = new StreamWriter(response)) {
                    await writer.WriteAsync("<h1>Middleware2</h1>");
                }

                await next.Invoke(env);
            };
            return appFunc;
        }

        public AppFunc Middleware3(AppFunc next) {
            AppFunc appFunc = async (IDictionary<string, object> env) => {
                // IOwinContext provides a strongly typed object interface to the environment
                IOwinContext context = new OwinContext(env);
                await context.Response.WriteAsync("<h1>Middleware3</h1>");
                await next.Invoke(env);
            };
            return appFunc;
        }
    }

    /// <summary>
    /// A middleware class demonstration.
    /// Notice: _next captures the AppFunc to continue the pipeline
    ///         Invoke takes the environment and acts on it, then continues with pipeline
    /// </summary>
    public class Middleware4 {
        AppFunc _next;
        public Middleware4(AppFunc next) {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env) {
            IOwinContext context = new OwinContext(env);
            await context.Response.WriteAsync("<h1>Middleware4</h1>");
            await _next.Invoke(env);
        }
    }

    public class Middleware5 {
        AppFunc _next;
        string _greeting;

        public Middleware5(AppFunc next, string greeting) {
            _next = next;
            _greeting = greeting;
        }

        public async Task Invoke(IDictionary<string, object> env) {
            IOwinContext context = new OwinContext(env);

            await context.Response.WriteAsync(string.Format("<h1>{0}</h2>", _greeting));
            await _next.Invoke(env);
        }
    }

    public class Middleware6 {
        AppFunc _next;
        MiddlewareConfigOptions _options;

        public Middleware6(AppFunc next, MiddlewareConfigOptions configOptions) {
            _next = next;
            _options = configOptions;
        }

        public async Task Invoke(IDictionary<string, object> env) {
            IOwinContext context = new OwinContext(env);

            await context.Response.WriteAsync(string.Format("<h1>{0}</h2>", _options.GetGreeting()));
            await _next.Invoke(env);
        }
    }

    public class MiddlewareConfigOptions {
        string _greetingTextFormat = "{0} from {1}{2}";
        public string GreetingText { get; set; }
        public string Greeter { get; set; }
        public DateTime Date { get; set; }

        public bool IncludeDate { get; set; }

        public MiddlewareConfigOptions(string greeting, string greeter) {
            GreetingText = greeting;
            Greeter = greeter;
            Date = DateTime.Now;
        }

        public string GetGreeting() {
            string DateText = "";
            if (IncludeDate) {
                DateText = string.Format(" on {0}", Date.ToShortDateString());
            }
            return string.Format(_greetingTextFormat, GreetingText, Greeter, DateText);
        }
    }

    public static class AppBuilderExtensions {
        public static void UseMiddleware4(this IAppBuilder app) {
            app.Use<Middleware4>();
        }

        public static void UseMiddleware5(this IAppBuilder app, string greeting) {
            app.Use<Middleware5>(greeting);
        }

        public static void UseMiddleware6(this IAppBuilder app, MiddlewareConfigOptions configOptions) {
            app.Use<Middleware6>(configOptions);
        }
    }
}
