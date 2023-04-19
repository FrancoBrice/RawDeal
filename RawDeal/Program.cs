using RawDeal;
using RawDealView;

const string DeckFolder = "05-SuperstarAbilities";

View view = View.BuildConsoleView();
string deckFolder = Path.Combine("data", DeckFolder);
Game game = new Game(view, deckFolder);
game.Play();
