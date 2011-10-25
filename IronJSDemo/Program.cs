using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronJSDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx = new IronJS.Hosting.CSharp.Context();
            ImageConstructor.AttachToContext(ctx);

            ctx.CreatePrintFunction();
            ctx.Execute(@"
                var img = new Image(100, 100); 
                print(img); // [object Image]
                img.clearWhite();
                img.drawElipse(5, 5, 20, 20);
                img.drawElipse(50, 50, 70, 70);
                img.saveFile('test.bmp');
            ");

            var img = ctx.GetGlobalAs<ImageObject>("img");
        }
    }
}
