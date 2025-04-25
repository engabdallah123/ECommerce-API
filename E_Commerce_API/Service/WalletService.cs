using Models.DTO.Wallet;
using Unit_Of_Work;

namespace E_Commerce_API.Service
{
    public class WalletService
    {
        UnitWork unitWork;
        public WalletService(UnitWork unitWork)
        {
            this.unitWork = unitWork;
        }
        public WalletDTO GetWalletByUserName(string username)
        {

            var wallet =  unitWork.db.Wallets.FirstOrDefault(w => w.UserName == username);
            if (wallet == null)
                throw new Exception("Wallet not found");
            else
            {
                WalletDTO walletDTO = new WalletDTO()
                {

                    UserId = wallet.UserId,
                    Balance = wallet.Balance,
                    UserName = wallet.UserName
                };
                return walletDTO;
            }

        }
        public void AddMoney(int id, WalletDTO walletDTO)
        {
            if (walletDTO == null)
                throw new Exception("Invalid Data");
            var wallet = unitWork.WalletRepo.GetById(id);
            if (wallet == null)
                throw new Exception("Wallet not found");
            else
            {
                
                wallet.Balance += walletDTO.Balance ?? 0; 
                unitWork.WalletRepo.Update(wallet, id);
                unitWork.Save();
            }
        }
        public void DeletWallet(int id)
        {
            var wallet = unitWork.WalletRepo.GetById(id);
            if (wallet == null)
                throw new Exception("Wallet not found");
            else
            {
                unitWork.WalletRepo.Delete(id);
                unitWork.Save();
            }
        }
    }
}
