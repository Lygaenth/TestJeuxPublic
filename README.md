# TestJeuxPublic
Public version
This is a test project for a video game created from scratch with WPF. Here is more of a framework of the game and an editor.
This is a copy of a private version that implements a bit more, I especially removed some models assets that.

Initially I just wanted to mess around with an architecture similar to what I have at work but where the legacy code is really heavy to refactor.
Video game can quicky gros in complexity and I sarted adding feature after feature so there's a lot of thing in this project.

In ended up adding another project to make a level editor.
I then tried to do make the same editor with blazor to learn how it works and train myself again in web.
Then I decided I needed to practice DDD in my project and starting refactoring heavily the solution.

So the current state is this:
- A bounded context for the game that still needs a lot of work (lack of repository, anemic model and very light unit test coverage)
- A WPF UI that works fine with this bounded context
- A WPF level editor that would need its own bounded context, thus some the game bounded context is adapted to both and could be simplified
- A web level editor using blazor but that can display a level for now and modify it locally but cannot publish the change yet (also there are remains of the base demo project)

Next goals:
- Fixing bugs
- Better unit test coverage
- Going to the end of the DDD refactorisation and creating a separate context for editors
- Fixing new bugs created by the refactoring
- Creating a true database instead of xml files
- Completing the two editors
- Adding features

Long term goal:
- Implementing the game in web browser
- Implementing the game in Unity
