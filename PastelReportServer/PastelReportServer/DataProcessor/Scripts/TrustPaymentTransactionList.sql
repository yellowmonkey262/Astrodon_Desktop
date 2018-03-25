select t.AutoNumber,
       t.DDate as "TransactionDate",
       t.LinkAcc as "LedgerAccount",
	   m.AccDesc as "LedgerAccountName",
       t.AccNumber as "Account",
	   m2.AccDesc as "AccountName",
       t.Refrence as "Reference",
       t.Description,
       t.Amount      
from [DataSet].LedgerTransactions t 
join [DataSet].LedgerMaster m on t.LinkAcc = m.AccNumber
join [DataSet].LedgerMaster m2 on t.AccNumber = m2.AccNumber
where  PPeriod >= 101 and PPeriod <= 112
and t.LinkAcc = '[TRUSTACCOUNT]'
and t.Amount < 0