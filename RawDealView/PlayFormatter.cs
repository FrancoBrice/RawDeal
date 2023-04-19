namespace RawDealView;

class PlayFormatter:ItemFormatter
{
    protected override string GetTextIndicatingThatTheListOfItemsIsEmpty()
        => "No hay nada que puedas jugar";
}