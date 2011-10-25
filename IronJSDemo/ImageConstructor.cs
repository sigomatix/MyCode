using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronJS;
using IronJS.Native;
using IronJS.Hosting;

using Env = IronJS.Environment;
using FO = IronJS.FunctionObject;
using CO = IronJS.CommonObject;

namespace IronJSDemo
{
    public static class ImageConstructor
    {
        public static void AttachToContext(CSharp.Context context)
        {
            var prototype = context.Environment.NewObject();
            var constructor = Utils.CreateConstructor<Func<FO, CO, double, double, CO>>(context.Environment, 2,Construct);
            var clearWhite = Utils.CreateFunction<Action<FO, CO>>(context.Environment, 0, ImageObject.ClearWhite);
            var saveFile = Utils.CreateFunction<Func<FO, CO, string, bool>>(context.Environment, 1, ImageObject.SaveFile);
            var drawElipse = Utils.CreateFunction<Action<FO, CO, double, double, double, double>>(context.Environment, 4, ImageObject.DrawElipse);

            prototype.Prototype = context.Environment.Prototypes.Object;
            prototype.Put("clearWhite", clearWhite, DescriptorAttrs.Immutable);
            prototype.Put("saveFile", saveFile, DescriptorAttrs.Immutable);
            prototype.Put("drawElipse", drawElipse, DescriptorAttrs.Immutable);
            constructor.Put("prototype", prototype, DescriptorAttrs.Immutable);
            context.SetGlobal("Image", constructor);
        }

        static CO Construct(FO ctor, CO _, double width, double height)
        {
            var prototype = ctor.GetT<CO>("prototype");
            return new ImageObject(width, height, ctor.Env, prototype);
        }
    }
}
