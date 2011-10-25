using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronJS;

using Env = IronJS.Environment;
using FO = IronJS.FunctionObject;
using CO = IronJS.CommonObject;

namespace IronJSDemo
{
    public class ImageObject : CommonObject
    {
        Image image;
        Graphics graphics;

        public ImageObject(double width, double height, Env env, CommonObject prototype)
            : base(env, env.Maps.Base, prototype)
        {
            Put("width", (double)width, DescriptorAttrs.Immutable);
            Put("height", (double)height, DescriptorAttrs.Immutable);
            image = new Bitmap((int)width, (int)height);
            graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black);
        }

        public override string ClassName
        {
            get
            {
                return "Image";
            }
        }

        internal static void ClearWhite(FO _, CO that)
        {
            var self = that.CastTo<ImageObject>();
            self.graphics.Clear(Color.White);
        }

        internal static void DrawElipse(FO func, CO that, double x, double y, double width, double height)
        {
            var self = that.CastTo<ImageObject>();
            var p = new Pen(Color.Black, 5);
            var rect = new Rectangle((int)x, (int)y, (int)width, (int)height);
            self.graphics.DrawEllipse(p, rect);
        }

        internal static bool SaveFile(FO func, CO that, string name)
        {
            if (name == null)
                return func.Env.RaiseTypeError<bool>("Name was null");

            var self = that.CastTo<ImageObject>();
            self.image.Save(name);
            return true;
        }
    }
}
