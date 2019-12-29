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
  `item_attack_physical` int(10) NOT NULL DEFAULT '0',
  `item_attack_magic` int(10) NOT NULL DEFAULT '0',
  `item_defense_physical` int(10) NOT NULL DEFAULT '0',
  `item_defense_magic` int(10) NOT NULL DEFAULT '0',
  `item_stats_strength` int(10) NOT NULL DEFAULT '0',
  `item_stats_Intelligence` int(10) NOT NULL DEFAULT '0',
  `item_stats_health` int(10) NOT NULL DEFAULT '0',
  `item_stats_mentality` int(10) NOT NULL DEFAULT '0',
  `item_exo` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`index`),
  UNIQUE KEY `item_id` (`item_id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 테이블 데이터 mdnf.hell_items:~13 rows (대략적) 내보내기
/*!40000 ALTER TABLE `hell_items` DISABLE KEYS */;
INSERT INTO `hell_items` (`index`, `item_id`, `item_name`, `item_attack_physical`, `item_attack_magic`, `item_defense_physical`, `item_defense_magic`, `item_stats_strength`, `item_stats_Intelligence`, `item_stats_health`, `item_stats_mentality`, `item_exo`) VALUES
	(1, '6001', '超 미틈 - 치닫지 못한 서리', 0, 0, 0, 0, 28, 28, 0, 31, 322),
	(2, '6002', '흑운 : 삼켜지는 태양', 0, 0, 0, 5914, 45, 0, 112, 0, 536),
	(3, '6003', '超 누리 - 끊이지 않는 생명', 0, 0, 2930, 0, 46, 46, 0, 52, 322),
	(4, '6004', '흑익 : 잠식되는 태양', 0, 0, 0, 0, 44, 44, 44, 44, 429),
	(5, '6005', '흑풍 : 갈라지는 태양', 0, 0, 0, 0, 66, 66, 66, 66, 429),
	(6, '6006', '超 물오름 - 늘푸르게 익는 산들', 0, 0, 1954, 0, 37, 37, 0, 41, 322),
	(7, '6007', '흑조 : 갈라지는 태양', 0, 0, 0, 9856, 0, 45, 0, 112, 536),
	(8, '6008', '超 하늘연 - 꺼지지 않는 밝음', 0, 0, 2442, 0, 46, 46, 0, 52, 322),
	(9, '6009', '흑염 : 잠식되는 태양', 0, 0, 0, 3942, 0, 67, 67, 0, 536),
	(10, '6010', '超 매듭 - 얽히지 않은 마음', 0, 0, 1465, 0, 28, 28, 0, 31, 322),
	(11, '6011', '흑얼 : 삼켜지는 태양', 0, 0, 0, 0, 66, 66, 66, 66, 429),
	(12, '6012', '現 : 흑천의 주인 - 대검', 1406, 1054, 0, 0, 78, 0, 0, 0, 858),
	(13, '6013', '現 : 흑천의 주인 - 광검', 1089, 1054, 0, 0, 78, 0, 0, 0, 858);
/*!40000 ALTER TABLE `hell_items` ENABLE KEYS */;

-- 테이블 mdnf.inventory 구조 내보내기
CREATE TABLE IF NOT EXISTS `inventory` (
  `index` int(10) NOT NULL AUTO_INCREMENT,
  `user_id` varchar(50) NOT NULL,
  `equip_00` varchar(10) NOT NULL DEFAULT '0',
  `equip_01` varchar(10) NOT NULL DEFAULT '0',
  `equip_02` varchar(10) NOT NULL DEFAULT '0',
  `equip_03` varchar(10) NOT NULL DEFAULT '0',
  `equip_04` varchar(10) NOT NULL DEFAULT '0',
  `equip_05` varchar(10) NOT NULL DEFAULT '0',
  `equip_06` varchar(10) NOT NULL DEFAULT '0',
  `equip_07` varchar(10) NOT NULL DEFAULT '0',
  `equip_08` varchar(10) NOT NULL DEFAULT '0',
  `equip_09` varchar(10) NOT NULL DEFAULT '0',
  `equip_10` varchar(10) NOT NULL DEFAULT '0',
  `equip_11` varchar(10) NOT NULL DEFAULT '0',
  `user_inventory` varchar(320) DEFAULT NULL,
  PRIMARY KEY (`index`),
  KEY `FK_LOGIN_USER_ID` (`user_id`),
  CONSTRAINT `FK_LOGIN_USER_ID` FOREIGN KEY (`user_id`) REFERENCES `login` (`user_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 테이블 데이터 mdnf.inventory:~7 rows (대략적) 내보내기
/*!40000 ALTER TABLE `inventory` DISABLE KEYS */;
INSERT INTO `inventory` (`index`, `user_id`, `equip_00`, `equip_01`, `equip_02`, `equip_03`, `equip_04`, `equip_05`, `equip_06`, `equip_07`, `equip_08`, `equip_09`, `equip_10`, `equip_11`, `user_inventory`) VALUES
	(1, 'test', '6012', '0', '6008', '6006', '0', '0', '6002', '0', '0', '6011', '0', '6004', '6001,6002,6003,6004,6005,6007,6009,6010,6011,6012,6013,1006,1004,6002,1006,6007'),
	(2, 'aaaa', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', ''),
	(3, 'bbbb', '1004', '0', '6008', '0', '6001', '0', '0', '0', '0', '6011', '6005', '6004', '1005,6004,6005,1001,6003,1008,6012,6003,6006,6002,6007,6009,6010,6010,1002,1007,1004,6008'),
	(4, 'cccc', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', ''),
	(5, 'dddd', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', NULL),
	(6, 'eeee', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', NULL),
	(7, 'ffff', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', NULL);
/*!40000 ALTER TABLE `inventory` ENABLE KEYS */;

-- 테이블 mdnf.login 구조 내보내기
CREATE TABLE IF NOT EXISTS `login` (
  `index` int(10) NOT NULL AUTO_INCREMENT,
  `user_id` varchar(50) NOT NULL,
  `user_password` varchar(200) NOT NULL,
  `user_name` varchar(50) NOT NULL,
  `sign_date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`index`),
  UNIQUE KEY `user_id` (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 테이블 데이터 mdnf.login:~7 rows (대략적) 내보내기
/*!40000 ALTER TABLE `login` DISABLE KEYS */;
INSERT INTO `login` (`index`, `user_id`, `user_password`, `user_name`, `sign_date`) VALUES
	(1, 'test', 'XzZ7zCBwUiKeNXHCKjpOkw==', 'test', '2019-12-24 02:56:35'),
	(2, 'aaaa', 'SqANas35DP0XuHNNJGuhCw==', 'aaaa', '2019-12-24 02:56:37'),
	(3, 'bbbb', 'ALTtUF7j0/OCnnyHzoH8HQ==', 'bbbb', '2019-12-24 02:56:39'),
	(4, 'cccc', 'HCRA8qbj89ciVsgpFBmS4w==', 'cccc', '2019-12-24 02:56:42'),
	(5, 'dddd', 'A4zZZA6jxfb4ZFiq7iaXPw==', 'dddd', '2019-12-24 02:56:44'),
	(6, 'eeee', 'DOsRBSmx3A5NmXcn5j7JVQ==', 'eeee', '2019-12-29 05:40:12'),
	(7, 'ffff', 'zQwAIFiWXN3zh5Objsggpw==', 'ffff', '2019-12-29 05:40:13');
/*!40000 ALTER TABLE `login` ENABLE KEYS */;

-- 테이블 mdnf.log_hell 구조 내보내기
CREATE TABLE IF NOT EXISTS `log_hell` (
  `index` int(10) NOT NULL AUTO_INCREMENT,
  `user_id` varchar(50) NOT NULL,
  `item_id` varchar(50) NOT NULL,
  `pick_up_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`index`),
  KEY `FK_LOG_HELL_USER_ID` (`user_id`),
  CONSTRAINT `FK_LOG_HELL_USER_ID` FOREIGN KEY (`user_id`) REFERENCES `login` (`user_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 테이블 데이터 mdnf.log_hell:~0 rows (대략적) 내보내기
/*!40000 ALTER TABLE `log_hell` DISABLE KEYS */;
INSERT INTO `log_hell` (`index`, `user_id`, `item_id`, `pick_up_time`) VALUES
	(1, 'test', '6005', '2019-12-30 02:38:47'),
	(2, 'test', '6005', '2019-12-30 02:38:45'),
	(3, 'test', '6007', '2019-12-30 02:38:23'),
	(4, 'test', '6007', '2019-12-30 02:38:34'),
	(5, 'bbbb', '6010', '2019-12-30 02:39:23'),
	(6, 'bbbb', '6010', '2019-12-30 02:39:42'),
	(7, 'bbbb', '6001', '2019-12-30 02:44:54'),
	(8, 'bbbb', '6008', '2019-12-30 02:45:31'),
	(9, 'bbbb', '6008', '2019-12-30 02:45:55');
/*!40000 ALTER TABLE `log_hell` ENABLE KEYS */;

-- 테이블 mdnf.result_items 구조 내보내기
CREATE TABLE IF NOT EXISTS `result_items` (
  `index` int(10) NOT NULL AUTO_INCREMENT,
  `item_id` varchar(50) NOT NULL,
  `item_name` varchar(50) NOT NULL,
  `item_attack_physical` int(10) NOT NULL DEFAULT '0',
  `item_attack_magic` int(10) NOT NULL DEFAULT '0',
  `item_defense_physical` int(10) NOT NULL DEFAULT '0',
  `item_defense_magic` int(10) NOT NULL DEFAULT '0',
  `item_stats_strength` int(10) NOT NULL DEFAULT '0',
  `item_stats_Intelligence` int(10) NOT NULL DEFAULT '0',
  `item_stats_health` int(10) NOT NULL DEFAULT '0',
  `item_stats_mentality` int(10) NOT NULL DEFAULT '0',
  `item_stats_exo` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`index`),
  UNIQUE KEY `item_index` (`item_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 테이블 데이터 mdnf.result_items:~8 rows (대략적) 내보내기
/*!40000 ALTER TABLE `result_items` DISABLE KEYS */;
INSERT INTO `result_items` (`index`, `item_id`, `item_name`, `item_attack_physical`, `item_attack_magic`, `item_defense_physical`, `item_defense_magic`, `item_stats_strength`, `item_stats_Intelligence`, `item_stats_health`, `item_stats_mentality`, `item_stats_exo`) VALUES
	(1, '1001', '조악한 가죽벨트', 0, 0, 100, 80, 3, 3, 3, 3, 3),
	(2, '1002', '조악한 광검', 50, 50, 0, 0, 5, 5, 5, 5, 5),
	(3, '1003', '조악한 가죽코트', 0, 0, 80, 0, 0, 0, 80, 0, 8),
	(4, '1004', '조악한 대검', 60, 30, 0, 0, 10, 0, 0, 0, 5),
	(5, '1005', '조악한 어깨견장', 0, 0, 45, 0, 0, 5, 5, 0, 5),
	(6, '1006', '조악한 가죽바지', 0, 0, 70, 0, 2, 1, 3, 4, 5),
	(7, '1007', '조악한 가죽신발', 0, 0, 30, 0, 10, 5, 6, 7, 8),
	(8, '1008', 'TEST', 9999, 9999, 9999, 9999, 999, 999, 999, 999, 999);
/*!40000 ALTER TABLE `result_items` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
