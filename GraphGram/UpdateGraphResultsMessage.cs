using CommunityToolkit.Mvvm.Messaging.Messages;

namespace GraphGram;
public class UpdateGraphResultsMessage : ValueChangedMessage<GraphResults> {
    public UpdateGraphResultsMessage(GraphResults value) : base(value) { }
}
