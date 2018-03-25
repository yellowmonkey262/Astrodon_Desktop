select t.AutoNumber,
       t.DDate as "TransactionDate",
       t.AccNumber as "Account",
	   m.AccDesc as "AccountName",
       t.Refrence as "Reference",
       t.Description,
       t.Amount      
from [DataSet].LedgerTransactions t 
join [DataSet].LedgerMaster m on t.AccNumber = m.AccNumber
where  PPeriod >= 100 and PPeriod <= 112
and t.Amount > 0
and ([AccountList])
