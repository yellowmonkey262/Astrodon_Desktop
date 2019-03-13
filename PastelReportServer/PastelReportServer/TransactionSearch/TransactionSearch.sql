select AccNumber,LinkAcc,DDate,Refrence,Amount,Description
from [DataSet].LedgerTransactions
where DDate >= @FromDate 
  and DDate <= @ToDate