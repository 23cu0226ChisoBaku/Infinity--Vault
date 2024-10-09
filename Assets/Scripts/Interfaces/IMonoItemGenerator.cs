using UnityEngine;
internal interface IMonoItemGenerator<T> where T : MonoBehaviour
{
    /// <summary>
    /// ���O�ŃA�C�e�����擾����
    /// </summary>
    /// <param name="itemName">�A�C�e���̖��O</param>
    /// <returns>�A�C�e���R���e�i</returns>
    public T GenerateSingleItem(string itemName);

    /// <summary>
    /// ���O�ň��̐��̃A�C�e����z��ɓ���ĕԂ�
    /// </summary>
    /// <param name="itemName">�A�C�e���̖��O</param>
    /// <param name="generateNum">�������鐔</param>
    /// <returns>�A�C�e���R���e�i�̔z��</returns>
    public T[] GenerateItems(string itemName,int generateNum);
}