select t.AutoNumber,
       t.DDate as "TransactionDate",
       t.LinkAcc as "LedgerAccount",
	   m.AccDesc as "LedgerAccountName",
       t.Refrence as "Reference",
       t.Description,
       t.Amount      
from [DataSet].LedgerTransactions t 
join [DataSet].LedgerMaster m on t.LinkAcc = m.AccNumber
where  PPeriod >= 101 and PPeriod <= 112
and t.AccNumber like '84%'
and t.Amount < 0