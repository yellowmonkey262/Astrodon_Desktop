select m.CustomerCode, m.CustomerDesc,
  BalanceLast01, BalanceLast02 ,BalanceLast03 ,BalanceLast04 ,BalanceLast05 ,BalanceLast06, BalanceLast07, BalanceLast08, BalanceLast09, BalanceLast10, BalanceLast11,BalanceLast12,BalanceLast13,
  BalanceThis01,BalanceThis02, BalanceThis03, BalanceThis04, BalanceThis05, BalanceThis06, BalanceThis07, BalanceThis08, BalanceThis09, BalanceThis10, BalanceThis11, BalanceThis12,BalanceThis13
from [DataSet].CustomerMaster m 
where m.CustomerCode = '@CUSTOMERCODE'