using System;
using System.Collections.Generic;
using System.Text;

namespace commandline_parser
{
    class CommandLineParserConstant
    {
        public readonly string _prefix="-";
    }

    class CommandLineParser
    {
        public delegate int ParseArgmentString(string[] args, int index);
        System.Collections.Generic.Dictionary<string,ParseArgmentString>  _action=new System.Collections.Generic.Dictionary<string,ParseArgmentString>();

        public void Clear()
        {
            _action.Clear();
        }
        public void AddAction(string option_name,ParseArgmentString function)
        {
            _action.Add(option_name,function);
        }
        public void Parse(string[] args)
        {
            var index = 0;            
            while(index<args.Length){
                //memo: option_name_full = "-foo"
                var option_name_full = args[index];
                //memo: option_name = "foo"
                var option_name=option_name_full.Substring(1);

                if (! _action.ContainsKey(option_name))
                {
                    System.Console.WriteLine("Unknown option. {0}",option_name_full);
                    throw new System.Exception();
                }
                index = _action[option_name](args, index+1);
            }
        }
    }

    class MyOption
    {
        public System.Collections.Generic.List<string> _foo=new System.Collections.Generic.List<string>();
        public int x, y, z;
        public bool use;
        public int ParseFoo(string[] args, int index)
        {
            _foo.Add(args[index]);            
            return index+1;
        }
        public int ParseBar(string[] args, int index) {
            x = System.Convert.ToInt32(args[index]);
            y = System.Convert.ToInt32(args[index+1]);
            z = System.Convert.ToInt32(args[index+2]);
            return index + 3;
        }
        public int ParseBar(string[] args, int index)
        {
            var name = args[index].ToLower();
            if (name == "true")
            {
                use = true;
            }
            else if (name=="false"){
                use = false;
            }
            return index + 1;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var option = new MyOption();            
            var obj = new CommandLineParser();
            
            obj.AddAction("foo", option.ParseFoo);
            obj.AddAction("bar", option.ParseBar);
            obj.Parse(args);
            System.Console.WriteLine("hoge.exe -foo str -bar x y z -hoge true\n");
        }
    }
}
