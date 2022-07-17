```mermaid
  graph BT;

%% Overall Architecture of C# Files managing a player %%
%% NOTE: This is a future plan, PlayerController/AIManager don't exist yet %%

    %% Player Manager %%
      PlayerManager;

    %% AI Manager %%
      AIManager-->PlayerController;


    %% Player Controller(For non-AI controls of player) %%
    PlayerController-->PlayerManager;

    %% Player Input %%
    PlayerInput-->PlayerController;

    %% Player Stats %%
      PlayerStats-->PlayerManager;
      PlayerStats-->PlayerUI;

    %% Player UI %%
    PlayerUI;


```
