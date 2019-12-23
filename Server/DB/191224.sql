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

-- 테이블 mdnf.item 구조 내보내기
CREATE TABLE IF NOT EXISTS `item` (
  `index` int(10) NOT NULL AUTO_INCREMENT,
  `item_index` int(10) NOT NULL,
  `item_id` varchar(50) NOT NULL,
  PRIMARY KEY (`index`),
  UNIQUE KEY `item_index` (`item_index`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 테이블 데이터 mdnf.item:~13 rows (대략적) 내보내기
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` (`index`, `item_index`, `item_id`) VALUES
	(1, 10001, 'e0be90bbee72e43439649ee55df7f15d'),
	(2, 10002, '6b8719acc4832644b9a09323e7faf320'),
	(3, 10003, '6b5b0f71828e31541a516aaa8c5c0799'),
	(4, 10004, '400b4af0e05991f4f9840bf705a31f1b'),
	(5, 10005, 'fe045270910de804ab5b216c7c7622fe'),
	(6, 10006, 'ef2a13482a3718947b689a3809d779cb'),
	(7, 10007, '042fc638b18bdeb498eeb72f98bd2440'),
	(8, 10008, '744c5b43cc1798f4e8d93bfc14224dc0'),
	(9, 10009, '906935775bcb6274fa9b0cf6c7fa481f'),
	(10, 10010, '2698449d2cbb46f4e8fc497453d0210c'),
	(11, 10011, '68c69ad00b7bbf24e88672d6fd284ab6'),
	(12, 10012, 'fa8b82ed25dfa2d43a359d464860c369'),
	(13, 10013, '3e3204101a6544a45b34fdfeef547278');
/*!40000 ALTER TABLE `item` ENABLE KEYS */;

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

-- 테이블 데이터 mdnf.login:~8 rows (대략적) 내보내기
/*!40000 ALTER TABLE `login` DISABLE KEYS */;
INSERT INTO `login` (`index`, `user_id`, `user_password`, `user_name`, `sign_date`) VALUES
	(1, 'test', 'XzZ7zCBwUiKeNXHCKjpOkw==', 'test', '2019-12-24 02:56:35'),
	(2, 'aaaa', 'SqANas35DP0XuHNNJGuhCw==', 'aaaa', '2019-12-24 02:56:37'),
	(3, 'bbbb', 'ALTtUF7j0/OCnnyHzoH8HQ==', 'bbbb', '2019-12-24 02:56:39'),
	(4, 'cccc', 'HCRA8qbj89ciVsgpFBmS4w==', 'cccc', '2019-12-24 02:56:42'),
	(5, 'dddd', 'A4zZZA6jxfb4ZFiq7iaXPw==', 'dddd', '2019-12-24 02:56:44');
/*!40000 ALTER TABLE `login` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
