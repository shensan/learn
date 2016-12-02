Last	; file result if exist
	s AllRecord=""
	;DownLoad TC
	d BUILD
	s epis=$tr(epis,"E","")
	;i time1="" s time1=$p($h,",",2)
	;s time=time1
	;i QC'="",mid="P2",time1'="" s time=time1+60
	;Save Result
	i QC'="",$l(epis),$l(result) d file^MIF000(mi,sample,epis,surname,result,date,time,QC)
	i QC="",$l(epis),$l(result) d file^MIF000(mi,sample,epis,surname,result,date,time,QC)
	;i $l(epis),$d(^DHCMIFLoadList(mi,"TRAN",epis)) k ^DHCMIFLoadList(mi,"TRAN",epis)
	q