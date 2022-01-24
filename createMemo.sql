CREATE TABLE IF NOT EXISTS `dc_notemanage` (
  `noteSeq` varchar(20) NOT NULL COMMENT '번호',
  `userNm` varchar(10) DEFAULT '' COMMENT '이름',
  `memo` varchar(4000) DEFAULT '' COMMENT 'Memo',
  `flagYN` varchar(1) DEFAULT 'Y' COMMENT '가용여부(Y:유효/N:삭제)',
  `regDate` datetime DEFAULT NULL COMMENT '최초저장일',
  `issueDate` datetime DEFAULT NULL COMMENT '최종저장일',
  `issueID` varchar(20) DEFAULT '' COMMENT '최종저장자 ID'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='사용자 관리';