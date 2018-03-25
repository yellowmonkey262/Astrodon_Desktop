
select m.CustomerCode, m.CustomerDesc,

  BalanceLast01, BalanceLast02 ,BalanceLast03 ,BalanceLast04 ,BalanceLast05 ,BalanceLast06, BalanceLast07, BalanceLast08, BalanceLast09, BalanceLast10, BalanceLast11,BalanceLast12,BalanceLast13,
  BalanceThis01,BalanceThis02, BalanceThis03, BalanceThis04, BalanceThis05, BalanceThis06, BalanceThis07, BalanceThis08, BalanceThis09, BalanceThis10, BalanceThis11, BalanceThis12,BalanceThis13,
  
  Sum(case WHEN LinkAcc = '1000000' and Description != 'Interest Charged' then IsNull(AMOUNT,0) else 0 end ) as Levy,
  Sum(case WHEN LinkAcc = '1045000'  and Description != 'Interest Charged' then IsNull(AMOUNT,0) else 0 end ) as Water,
  Sum(case WHEN LinkAcc = '1010000'  and Description != 'Interest Charged' then IsNull(AMOUNT,0) else 0 end ) as Electricity ,
  Sum(case WHEN Description = 'Interest Charged'  then IsNull(AMOUNT,0) else 0 end ) as Interest ,
  Sum(case WHEN LinkAcc = '1030000'  and Description != 'Interest Charged' then IsNull(AMOUNT,0) else 0 end ) as Legal,
  Sum(case WHEN LinkAcc = '1020000'  and Description != 'Interest Charged' then IsNull(AMOUNT,0) else 0 end ) as Sewer,
  Sum(case WHEN LinkAcc like '84%'  and Description != 'Interest Charged' then IsNull(AMOUNT,0) else 0 end ) as Payments,
  Sum(case WHEN LinkAcc = '1085000'  and Description != 'Interest Charged' then IsNull(AMOUNT,0) else 0 end ) as CSOS,
  Sum(case WHEN (LinkAcc in ('1000000','1045000','1010000','1000000','1030000','1020000','1085000')
             or LinkAcc like '84%' or Description = 'Interest Charged' ) then 0 else AMOUNT end) as Sundries
  
from [DataSet].CustomerMaster m left join 
     [DataSet].LedgerTransactions t on  t.AccNumber = m.CustomerCode
where PPeriod = ?
  %CUSTOMERCODEFILTER%
group by  m.CustomerCode, m.CustomerDesc, BalanceThis01,
  BalanceThis01,BalanceThis02, BalanceThis03, BalanceThis04, BalanceThis05, BalanceThis06, BalanceThis07, BalanceThis08, BalanceThis09, BalanceThis10, BalanceThis11, BalanceThis12,BalanceThis13,
  BalanceLast01, BalanceLast02 ,BalanceLast03 ,BalanceLast04 ,BalanceLast05 ,BalanceLast06, BalanceLast07, BalanceLast08, BalanceLast09, BalanceLast10, BalanceLast11,BalanceLast12,BalanceLast13