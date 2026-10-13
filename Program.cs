namespace ConsoleATM;

class Program
/*
 * ATM
 * 1. Pin benar: 1234, maks percobaan 3; Jika salah 3x, kartu diblokir
 * 2. Saldo awal 1.000.000. 
 * 3. Menu: 1 Cek saldo, 2 Setor, 3 Tarik, 4 Riwayat, 0 Keluar.
 * 4. Jumlah penarikan dan setor harus kelipatan 50k
 * 5. riwayat 5 transaksi terakhir -> jika history penuh, hilangkan yang paling lama
 * 6. Method minimal: TampilkanMenu(), ValidasiSetor(jumlah), ValidasiTarik(jumlah, saldo) yang mengembalikan bool, dan TambahRiwayat(...)
 */
{
    private static int balance = 1000000;
    private static string[] history = new string[5];
    private const string PIN = "1234";
    
    static void Main(string[] args)
    {
        bool isAuth = HasAuthenticate();
        if (!isAuth)
        {
            Console.WriteLine("Kartu diblokir");
            return;
        }

        do
        {
            ShowMenu();
            var option = GetOption();
            Console.WriteLine("==============================");
            switch (option)
            {
                case "1":
                    PrintBalance();
                    break;
                case "2":
                    Deposit();
                    break;
                case "3":
                    Withdraw();
                    break;
                case "4":
                    ShowHistory();
                    break;
                case "0":
                    break;
                default:
                    Console.WriteLine("Opsi tidak valid; Pilih 0-4.");
                    break;
            }

            if (option == "0")
            {
                Console.WriteLine("Terima kasih");
                return;
            }

            Console.WriteLine("===== Enter untuk melanjutkan =====");
            Console.ReadLine();
        } while (true);
    }

    static void Deposit()
    {
        Console.WriteLine("Input jumlah setoran: ");
        var depoAmount = GetAmount();
        
        if (!IsAmountValid(depoAmount))
        {
            Console.WriteLine("Deposit gagal: Nilai deposit harus kelipatan 50.000 dan diatas 0");
        }
        else
        {
            balance += depoAmount;
            Console.WriteLine("Deposit berhasil\nSisa saldo: " + balance);
            history = PushHistory(history, $"+ {depoAmount}");
        }
    }

    static void Withdraw()
    {
        Console.WriteLine("Input jumlah penarikan: ");
        int withdrawAmount = GetAmount();
        
        if (!IsAmountValid(withdrawAmount))
        {
            Console.WriteLine("Penarikan gagal: Nilai penarikan harus kelipatan 50.000 dan diatas 0");
        }
        else if (withdrawAmount > balance)
        {
            Console.WriteLine($"Penarikan gagal: Saldo tidak cukup\nSaldo: {balance}\nWithdraw: {withdrawAmount}");
        }
        else
        {
            balance -= withdrawAmount;
            Console.WriteLine("Penarikan berhasil\nSisa saldo: " + balance);
            history = PushHistory(history, $"- {withdrawAmount}");
        }
    }

    static void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("Menu:");
        Console.WriteLine("1. Cek Saldo");
        Console.WriteLine("2. Setor");
        Console.WriteLine("3. Tarik");
        Console.WriteLine("4. Riwayat");
        Console.WriteLine("0. Keluar");
        Console.Write("Pilihan: ");
    }

    static string GetOption()
    {
        var option = Console.ReadLine();
        return option;
    }

    static bool HasAuthenticate()
    {
        var counter = 1;
        while (counter <= 3)
        {
            AuthPrompt(counter);
            var input =  Console.ReadLine();
            if (input == PIN)
            {
                return true;
            }
            counter++;
        }

        return false;
    }

    static void AuthPrompt(int counter)
    {
        Console.Clear();
        Console.WriteLine("Percobaan: " + counter + "/3");
        
        // tampil di percobaan kedua dan tiga
        if (counter > 1)
        {
            Console.WriteLine("Pin salah. Coba lagi");
        }
        Console.Write("Masukkan pin: ");
    }
    
    static void PrintBalance()
    {
        Console.WriteLine("Sisa saldo:  " + balance);
    }

    static int GetAmount()
    {
        try
        {
            return Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            return -1;
        }
    }

    static bool IsAmountValid(int amount)
    {
        return amount % 50000 == 0 && amount > 0;
    }
    
    // Geser history lama (arr)
    static string[] PushHistory(string[] arr, string amount)
    {
        string[] newArray = new string[5];
        newArray[0] = arr[1];
        newArray[1] = arr[2];
        newArray[2] = arr[3];
        newArray[3] = arr[4];
        newArray[4] = amount;
        
        return newArray;
    }

    static void ShowHistory()
    {
        foreach (var item in history)
        {
            Console.WriteLine(item);
        }
    }
}