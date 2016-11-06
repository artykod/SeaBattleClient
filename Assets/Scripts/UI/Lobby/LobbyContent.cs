using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyContent : BindModel
{
    private enum SortingColumn
    {
        Name,
        Currency,
        Value,
    }

    private enum SortingDirection
    {
        Up,
        Down,
    }

    private LobbyBattleList _battleList;
    private Transform _itemsContent;
    private Transform _createBattleItemsContent;
    private Transform _contentTransform;
    private Data.Lobby _lobby;
    private SortingColumn _sortingColumn = SortingColumn.Value;
    private SortingDirection _sortingDirection = SortingDirection.Down;

    protected override Transform Content { get { return _contentTransform ?? transform; } }

    public LobbyContent(Data.Lobby lobby) : base("UI/Lobby/LobbyContent")
    {
        _lobby = lobby;

        _itemsContent = transform.FindChild("Items");
        _createBattleItemsContent = transform.FindChild("CreateBattleItems");

        _contentTransform = _createBattleItemsContent;
        AddLast(new LobbyCreateBattleWithPlayerItem());
        AddLast(new LobbyCreateBattleWithCPUItem());
        _contentTransform = transform;

        _contentTransform = _itemsContent;
        AddLast(_battleList = new LobbyBattleList(SortLobby(_lobby)));
        _itemsContent.GetComponent<ScrollRect>().content = _battleList.transform as RectTransform;
        _contentTransform = transform;
    }

    public void UpdateData(Data.Lobby lobby)
    {
        _lobby = lobby;
        _battleList.UpdateData(SortLobby(_lobby));
    }

    [BindCommand]
    private void Back()
    {
        Root.Destroy();
    }

    private LobbyBattleList.SortedLobbyMatch[] SortLobby(Data.Lobby lobby)
    {
        var battles = new List<LobbyBattleList.SortedLobbyMatch>(lobby.Count);
        foreach (var i in lobby) battles.Add(new LobbyBattleList.SortedLobbyMatch(i.Key, i.Value));

        switch (_sortingColumn)
        {
            case SortingColumn.Name:
                battles.Sort((a, b) => a.Match.Sides[0].Nick.CompareTo(b.Match.Sides[0].Nick));
                break;
            case SortingColumn.Currency:
                battles.Sort((a, b) => a.Match.Bet.Type.CompareTo(b.Match.Bet.Type));
                break;
            case SortingColumn.Value:
                battles.Sort((a, b) => a.Match.Bet.Value.CompareTo(b.Match.Bet.Value));
                break;
        }

        var array = new LobbyBattleList.SortedLobbyMatch[battles.Count];
        for (int i = 0; i < array.Length; i++)
        {
            var index = _sortingDirection == SortingDirection.Down ? i : array.Length - i - 1;
            array[index] = battles[i];
        }

        return array;
    }

    private void SortBy(SortingColumn column, SortingDirection direction)
    {
        if (_sortingColumn == column && _sortingDirection == direction) return;

        _sortingColumn = column;
        _sortingDirection = direction;

        UpdateData(_lobby);
    }

    [BindCommand]
    private void SortUpName() { SortBy(SortingColumn.Name, SortingDirection.Up); }
    [BindCommand]
    private void SortDownName() { SortBy(SortingColumn.Name, SortingDirection.Down); }
    [BindCommand]
    private void SortUpCurrency() { SortBy(SortingColumn.Currency, SortingDirection.Up); }
    [BindCommand]
    private void SortDownCurrency() { SortBy(SortingColumn.Currency, SortingDirection.Down); }
    [BindCommand]
    private void SortUpValue() { SortBy(SortingColumn.Value, SortingDirection.Up); }
    [BindCommand]
    private void SortDownValue() { SortBy(SortingColumn.Value, SortingDirection.Down); }
}
