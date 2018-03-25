
select l.AccNumber,l.AccDesc, 
       m.CustomerCode, m.CustomerDesc,
       t.DDate,t.Description,t.Amount
from [DataSet].CustomerMaster m join 
     [DataSet].LedgerTransactions t on  t.AccNumber = m.CustomerCode join
     [DataSet].LedgerMaster l on l.AccNumber = t.LinkAcc
where t.LinkAcc not in ('1000000','1045000','1010000','1030000','1020000','1085000') 
  and t.LinkAcc not like '84%'
  and t.Description != 'Interest Charged'
  and t.PPeriod = ?
order by l.AccDesc,L.AccNumber,m.CustomerDesc,t.DDate