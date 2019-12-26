-- --------------------------------------------------------
-- 호스트:                          127.0.0.1
-- 서버 버전:                        8.0.18 - MySQL Community Server - GPL
-- 서버 OS:                        Win64
-- HeidiSQL 버전:                  10.3.0.5771
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- mdnf 데이터베이스 구조 내보내기
CREATE DATABASE IF NOT EXISTS `mdnf` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `mdnf`;

-- 테이블 mdnf.hell_items 구조 내보내기
CREATE TABLE IF NOT EXISTS `hell_items` (
  `index` int(10) NOT NULL AUTO_INCREMENT,
  `item_id` varchar(50) NOT NULL,
  `item_name` varchar(50) NOT NULL,
  PRIMARY KEY (`index`),
  UNIQUE KEY `item_id` (`item_id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 테이블 데이터 mdnf.hell_items:~0 rows (대략적) 내보내기
/*!40000 ALTER TABLE `hell_items` DISABLE KEYS */;
INSERT INTO `hell_items` (`index`, `item_id`, `item_name`) VALUES
	(1, '6001', '超 미틈 - 치닫지 못한 서리'),
	(2, '6002', '흑운 : 삼켜지는 태양'),
	(3, '6003', '超 누리 - 끊이지 않는 생명'),
	(4, '6004', '흑익 : 잠식되는 태양'),
	(5, '6005', '흑풍 : 갈라지는 태양'),
	(6, '6006', '超 물오름 - 늘푸르게 익는 산들'),
	(7, '6007', '흑조 : 갈라지는 태양'),
	(8, '6008', '超 하늘연 - 꺼지지 않는 밝음'),
	(9, '6009', '흑염 : 잠식되는 태양'),
	(10, '6010', '超 매듭 - 얽히지 않은 마음'),
	(11, '6011', '흑얼 : 삼켜지는 태양'),
	(12, '6012', '現 : 흑천의 주인 - 대검'),
	(13, '6013', '現 : 흑천의 주인 - 광검');
/*!40000 ALTER TABLE `hell_items` ENABLE KEYS */;

-- 테이블 mdnf.login 구조 내보내기
CREATE TABLE IF NOT EXISTS `login` (
  `index` int(10) NOT NULL AUTO_INCREMENT,
  `user_id` varchar(50) NOT NULL,
  `user_password` varchar(200) NOT NULL,
  `user_name` varchar(50) NOT NULL,
  `sign_date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`index`),
  UNIQUE KEY `user_id` (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 테이블 데이터 mdnf.login:~5 rows (대략적) 내보내기
/*!40000 ALTER TABLE `login` DISABLE KEYS */;
INSERT INTO `login` (`index`, `user_id`, `user_password`, `user_name`, `sign_date`) VALUES
	(1, 'test', 'XzZ7zCBwUiKeNXHCKjpOkw==', 'test', '2019-12-24 02:56:35'),
	(2, 'aaaa', 'SqANas35DP0XuHNNJGuhCw==', 'aaaa', '2019-12-24 02:56:37'),
	(3, 'bbbb', 'ALTtUF7j0/OCnnyHzoH8HQ==', 'bbbb', '2019-12-24 02:56:39'),
	(4, 'cccc', 'HCRA8qbj89ciVsgpFBmS4w==', 'cccc', '2019-12-24 02:56:42'),
	(5, 'dddd', 'A4zZZA6jxfb4ZFiq7iaXPw==', 'dddd', '2019-12-24 02:56:44');
/*!40000 ALTER TABLE `login` ENABLE KEYS */;

-- 테이블 mdnf.result_items 구조 내보내기
CREATE TABLE IF NOT EXISTS `result_items` (
  `index` int(10) NOT NULL AUTO_INCREMENT,
  `item_id` varchar(50) NOT NULL,
  `item_name` varchar(50) NOT NULL,
  PRIMARY KEY (`index`),
  UNIQUE KEY `item_index` (`item_id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 테이블 데이터 mdnf.result_items:~8 rows (대략적) 내보내기
/*!40000 ALTER TABLE `result_items` DISABLE KEYS */;
INSERT INTO `result_items` (`index`, `item_id`, `item_name`) VALUES
	(1, '1001', '조악한 가죽벨트'),
	(2, '1002', '조악한 광검'),
	(3, '1003', '조악한 가죽코트'),
	(4, '1004', '조악한 대검'),
	(5, '1005', '조악한 어깨견장'),
	(6, '1006', '조악한 가죽바지'),
	(7, '1007', '조악한 가죽신발'),
	(8, '1008', 'TEST');
/*!40000 ALTER TABLE `result_items` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
