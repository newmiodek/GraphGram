using CommunityToolkit.Mvvm.Messaging.Messages;

namespace GraphGram;
public class ImportDataMessage : ValueChangedMessage<float?[,]> {
    public ImportDataMessage(float?[,] value) : base(value) { }
}
