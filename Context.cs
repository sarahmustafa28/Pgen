using System.Collections.Generic;
using System.Collections.Immutable;
using LLVMSharp.Interop;

namespace parser;

public class Context
{
    public ImmutableDictionary<string, LLVMValueRef> _source;

    public static Context Empty => new Context();

    public Context() => _source = ImmutableDictionary<string, LLVMValueRef>.Empty;

    public Context(ImmutableDictionary<string, LLVMValueRef> source) => _source = source;

    public Context Add(string key, LLVMValueRef value)
        => new Context(_source.SetItem(key, value));

    public Context AddArguments(LLVMValueRef function, List<string> arguments)
    {
        if (arguments.Count == 0)
            return this;

        var s = _source;

        for (int i = 0; i < arguments.Count; i++)
        {
            var name = arguments[i];
            var param = function.GetParam((uint)i);
            param.Name = name;
            s = s.SetItem(name, param);
        }

        return new Context(s);
    }

    public LLVMValueRef? Get(string key)
    {
        if (_source.TryGetValue(key, out var value))
            return value;
        else
            return null;
    }
}