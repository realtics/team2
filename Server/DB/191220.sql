-- --------------------------------------------------------
-- 호스트:                          127.0.0.1
-- 서버 버전:                        8.0.18 - MySQL Community Server - GPL
-- 서버 OS:                        Win64
-- HeidiSQL 버전:                  10.2.0.5599
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- mdnf 데이터베이스 구조 내보내기
CREATE DATABASE IF NOT EXISTS `mdnf` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `mdnf`;

-- 테이블 mdnf.login 구조 내보내기
CREATE TABLE IF NOT EXISTS `login` (
  `index` int(10) NOT NULL AUTO_INCREMENT,
  `user_id` varchar(50) NOT NULL,
  `user_password` varchar(200) NOT NULL,
  `user_name` varchar(50) NOT NULL,
  `sign_date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`index`),
  UNIQUE KEY `user_id` (`user_id`),
  UNIQUE KEY `user_name` (`user_name`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 테이블 데이터 mdnf.login:~5 rows (대략적) 내보내기
/*!40000 ALTER TABLE `login` DISABLE KEYS */;
INSERT INTO `login` (`index`, `user_id`, `user_password`, `user_name`, `sign_date`) VALUES
	(1, 'test', 'XzZ7zCBwUiKeNXHCKjpOkw==', '테스트', '2019-12-17 11:24:23'),
	(2, 'aaaa', 'SqANas35DP0XuHNNJGuhCw==', '에이에이', '2019-12-17 11:24:25'),
	(3, 'bbbb', 'ALTtUF7j0/OCnnyHzoH8HQ==', '비비비비', '2019-12-17 11:24:28'),
	(4, 'cccc', 'HCRA8qbj89ciVsgpFBmS4w==', '씨씨씨씨', '2019-12-19 12:28:30'),
	(5, 'dddd', 'A4zZZA6jxfb4ZFiq7iaXPw==', '디디디디', '2019-12-19 12:28:49');
/*!40000 ALTER TABLE `login` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
