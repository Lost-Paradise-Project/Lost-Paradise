using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Shared._LostParadise.EntityTable.EntitySelectors;

[TypeSerializer]
public sealed class LPEntityTableTypeSerializer :
    ITypeReader<LPEntityTableSelector, MappingDataNode>
{
    public ValidationNode Validate(ISerializationManager serializationManager,
        MappingDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        if (node.Has(LPEntSelector.IdDataFieldTag))
            return serializationManager.ValidateNode<LPEntSelector>(node, context);

        return new ErrorNode(node, "Custom validation not supported! Please specify the type manually!");
    }

    public LPEntityTableSelector Read(ISerializationManager serializationManager,
        MappingDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<LPEntityTableSelector>? instanceProvider = null)
    {
        var type = typeof(LPEntityTableSelector);
        if (node.Has(LPEntSelector.IdDataFieldTag))
            type = typeof(LPEntSelector);

        return (LPEntityTableSelector) serializationManager.Read(type, node, context)!;
    }
}
