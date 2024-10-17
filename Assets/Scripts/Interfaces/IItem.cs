/// <summary>
/// �A�C�e���̏��
/// </summary>
public struct ItemInfo
{
  //public Guid UniqueID;
  public string Name;

}

public struct VaultInfo
{
  public EPuzzleDifficulty Difficulty;
  public ItemInfo ItemInfo;
  public int ItemCount;
}
/// <summary>
/// �A�C�e�����擾����
/// �擾����A�C�e���̎�ނɂ���ď����𕪗�������
/// Visitor Pattern
/// </summary>
public interface IItemGetable
{
  void GetItem(GemContainer gem);
  void GetItem(KeyItemContainer keyItem);
  void GetItem(ConsumeItemContainer consumeItem);
}

/// <summary>
/// TODO
/// </summary>
internal interface IItemSettable
{
  void SetItem(ItemInfo item);
}
