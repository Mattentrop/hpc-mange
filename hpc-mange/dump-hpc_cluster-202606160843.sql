/*M!999999\- enable the sandbox mode */ 
-- MariaDB dump 10.19-12.2.2-MariaDB, for Linux (x86_64)
--
-- Host: 192.168.122.1    Database: hpc_cluster
-- ------------------------------------------------------
-- Server version	9.7.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*M!100616 SET @OLD_NOTE_VERBOSITY=@@NOTE_VERBOSITY, NOTE_VERBOSITY=0 */;

--
-- Table structure for table `Clusters`
--

DROP TABLE IF EXISTS `Clusters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `Clusters` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) NOT NULL,
  `Localizacao` varchar(150) NOT NULL,
  `CapacidadeRede` varchar(50) DEFAULT NULL,
  `PesquisadorId` int DEFAULT NULL,
  `created_by` int DEFAULT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleted_at` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_clusters_pesquisador` (`PesquisadorId`),
  KEY `fk_clusters_createdby` (`created_by`),
  CONSTRAINT `fk_clusters_createdby` FOREIGN KEY (`created_by`) REFERENCES `Usuarios` (`Id`) ON DELETE SET NULL,
  CONSTRAINT `fk_clusters_pesquisador` FOREIGN KEY (`PesquisadorId`) REFERENCES `Pesquisadores` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Clusters`
--

SET @OLD_AUTOCOMMIT=@@AUTOCOMMIT, @@AUTOCOMMIT=0;
LOCK TABLES `Clusters` WRITE;
/*!40000 ALTER TABLE `Clusters` DISABLE KEYS */;
INSERT INTO `Clusters` VALUES
(1,'Turing-Cluster','Sala Cofre A','200 Gbps Infiniband',1,NULL,'2026-06-16 01:00:24','2026-06-16 01:15:58',NULL),
(2,'Lovelace-Cluster','Sala Cofre B','100 Gbps Ethernet',1,NULL,'2026-06-16 01:00:24','2026-06-16 01:15:58',NULL),
(3,'Hopper-Clust','Datacenter Principal','400 Gbps Infiniband',1,NULL,'2026-06-16 01:00:24','2026-06-16 02:25:07',NULL),
(4,'Neumann-Cluster','Laboratorio de Fisica','10 Gbps Ethernet',1,NULL,'2026-06-16 01:00:24','2026-06-16 01:15:58',NULL),
(5,'Dijkstra-Cluster','Laboratorio de IA','100 Gbps Infiniband',1,NULL,'2026-06-16 01:00:24','2026-06-16 01:15:58',NULL),
(6,'Mongo-Cluster','Sala de Dados','10 Gbps',1,NULL,'2026-06-16 01:00:24','2026-06-16 01:15:58',NULL),
(7,'Offline-Cluster','Laboratório','1 Gbps',1,NULL,'2026-06-16 01:00:24','2026-06-16 01:15:58',NULL),
(8,'Turing-Node02','Sala Cofre A','200 Gbps',1,NULL,'2026-06-16 01:00:24','2026-06-16 01:15:58',NULL),
(9,'Feynman-Cluster','Rack Central 04','400 Gbps Infiniband',1,NULL,'2026-06-16 01:00:24','2026-06-16 01:15:58',NULL),
(10,'Turing-Node04','Sala Cofre B','200 Gbps',1,NULL,'2026-06-16 01:14:01','2026-06-16 02:25:07',NULL),
(11,'Feynman-Cluster','Rack Central 04','400 Gbps Infiniband',1,NULL,'2026-06-16 01:14:01','2026-06-16 01:28:51','2026-06-16 01:28:51'),
(12,'Cluster-testes','Sala-sub-secreta','100Gbps',1,NULL,'2026-06-16 01:28:41','2026-06-16 02:25:07','2026-06-16 01:49:07'),
(13,'Teste-cluster','Sala-23J','200Gbps',1,1,'2026-06-16 01:48:58','2026-06-16 03:09:26','2026-06-16 03:09:26'),
(14,'novo-cluster-plus','Ali atras','200Gbps',1,1,'2026-06-16 01:56:44','2026-06-16 03:09:35',NULL),
(15,'ola','olll','20Gbps',1,1,'2026-06-16 01:58:35','2026-06-16 02:25:07','2026-06-16 02:09:22'),
(16,'ola-test','aqui','200Gbps',1,1,'2026-06-16 02:07:22','2026-06-16 02:25:07','2026-06-16 02:12:19'),
(17,'Cluster-no-video','Video','200Gbps',1,1,'2026-06-16 03:10:49','2026-06-16 03:10:49',NULL),
(18,'Turing-Node02','Sala Cofre A','200 Gbps',1,1,'2026-06-16 03:11:19','2026-06-16 03:11:19',NULL),
(19,'Feynman-Cluster','Rack Central 04','400 Gbps Infiniband',1,1,'2026-06-16 03:11:19','2026-06-16 03:11:19',NULL);
/*!40000 ALTER TABLE `Clusters` ENABLE KEYS */;
UNLOCK TABLES;
COMMIT;
SET AUTOCOMMIT=@OLD_AUTOCOMMIT;

--
-- Table structure for table `Nodos`
--

DROP TABLE IF EXISTS `Nodos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `Nodos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Hostname` varchar(50) NOT NULL,
  `MemoriaRAM` int NOT NULL,
  `ModeloCPU` varchar(100) NOT NULL,
  `ClusterId` int NOT NULL,
  `created_by` int DEFAULT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleted_at` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Hostname` (`Hostname`),
  KEY `FK_Nodos_Clusters` (`ClusterId`),
  KEY `fk_nodos_createdby` (`created_by`),
  CONSTRAINT `FK_Nodos_Clusters` FOREIGN KEY (`ClusterId`) REFERENCES `Clusters` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_nodos_createdby` FOREIGN KEY (`created_by`) REFERENCES `Usuarios` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Nodos`
--

SET @OLD_AUTOCOMMIT=@@AUTOCOMMIT, @@AUTOCOMMIT=0;
LOCK TABLES `Nodos` WRITE;
/*!40000 ALTER TABLE `Nodos` DISABLE KEYS */;
INSERT INTO `Nodos` VALUES
(1,'turing-node01',512,'Intel Xeon Platinum 8380',1,NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(2,'turing-node02',512,'Intel Xeon Platinum 8380',1,NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(3,'lovelace-gpu01',256,'AMD EPYC 7763',2,NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(4,'lovelace-gpu02',256,'AMD EPYC 7763',2,NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(5,'hopper-dgx01',1024,'NVIDIA Grace Hopper',3,NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(6,'hopper-dgx02',1024,'NVIDIA Grace Hopper',3,NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(7,'neumann-compute01',128,'Intel Xeon Gold 6248',4,NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(8,'neumann-compute02',128,'Intel Xeon Gold 6248',4,NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(9,'dijkstra-ai01',512,'AMD Threadripper PRO',5,NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(10,'dijkstra-ai02',512,'AMD Threadripper PRO',5,NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(11,'turning-node03',32,'Intel Xeon / AMD EPYC',1,NULL,'2026-06-16 10:15:30','2026-06-16 10:15:30',NULL);
/*!40000 ALTER TABLE `Nodos` ENABLE KEYS */;
UNLOCK TABLES;
COMMIT;
SET AUTOCOMMIT=@OLD_AUTOCOMMIT;

--
-- Table structure for table `Nodos_Softwares`
--

DROP TABLE IF EXISTS `Nodos_Softwares`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `Nodos_Softwares` (
  `NodoId` int NOT NULL,
  `SoftwareId` int NOT NULL,
  `DataInstalacao` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`NodoId`,`SoftwareId`),
  KEY `FK_NodosSoftwares_Softwares` (`SoftwareId`),
  CONSTRAINT `FK_NodosSoftwares_Nodos` FOREIGN KEY (`NodoId`) REFERENCES `Nodos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_NodosSoftwares_Softwares` FOREIGN KEY (`SoftwareId`) REFERENCES `Softwares` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Nodos_Softwares`
--

SET @OLD_AUTOCOMMIT=@@AUTOCOMMIT, @@AUTOCOMMIT=0;
LOCK TABLES `Nodos_Softwares` WRITE;
/*!40000 ALTER TABLE `Nodos_Softwares` DISABLE KEYS */;
INSERT INTO `Nodos_Softwares` VALUES
(1,2,'2026-06-10 01:12:41'),
(1,3,'2026-06-10 01:12:41'),
(1,4,'2026-06-10 01:12:41'),
(2,2,'2026-06-10 01:12:41'),
(2,3,'2026-06-10 01:12:41'),
(2,4,'2026-06-10 01:12:41'),
(3,1,'2026-06-10 01:12:41'),
(3,5,'2026-06-10 01:12:41'),
(3,6,'2026-06-10 01:12:41'),
(4,1,'2026-06-10 01:12:41'),
(4,5,'2026-06-10 01:12:41'),
(4,6,'2026-06-10 01:12:41'),
(5,1,'2026-06-10 01:12:41'),
(5,7,'2026-06-10 01:12:41'),
(5,8,'2026-06-10 01:12:41'),
(6,1,'2026-06-10 01:12:41'),
(6,7,'2026-06-10 01:12:41'),
(6,8,'2026-06-10 01:12:41'),
(9,4,'2026-06-10 01:12:41'),
(9,5,'2026-06-10 01:12:41'),
(9,6,'2026-06-10 01:12:41'),
(10,4,'2026-06-10 01:12:41'),
(10,5,'2026-06-10 01:12:41'),
(10,6,'2026-06-10 01:12:41');
/*!40000 ALTER TABLE `Nodos_Softwares` ENABLE KEYS */;
UNLOCK TABLES;
COMMIT;
SET AUTOCOMMIT=@OLD_AUTOCOMMIT;

--
-- Table structure for table `Pesquisadores`
--

DROP TABLE IF EXISTS `Pesquisadores`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `Pesquisadores` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(150) NOT NULL,
  `Departamento` varchar(100) DEFAULT NULL,
  `Email` varchar(150) NOT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleted_at` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Pesquisadores`
--

SET @OLD_AUTOCOMMIT=@@AUTOCOMMIT, @@AUTOCOMMIT=0;
LOCK TABLES `Pesquisadores` WRITE;
/*!40000 ALTER TABLE `Pesquisadores` DISABLE KEYS */;
INSERT INTO `Pesquisadores` VALUES
(1,'Admin Sistema','Infraestrutura','admin@hpc.local','2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(2,'Carlos Silva','Engenharia de Dados','carlos.silva@hpc.local','2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(3,'Mariana Costa','Inteligencia Artificial','mariana.costa@hpc.local','2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(4,'Fernando Souza','Fisica Quantica','fernando.souza@hpc.local','2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(5,'Ana Oliveira','Biologia Computacional','ana.oliveira@hpc.local','2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(6,'Dr. Alan Turing','Ciência da Computação','alan@univap.br','2026-06-16 01:15:58','2026-06-16 01:15:58',NULL),
(7,'Dra. Margaret Hamilton','Engenharia de Software','margaret@univap.br','2026-06-16 01:15:58','2026-06-16 01:15:58',NULL);
/*!40000 ALTER TABLE `Pesquisadores` ENABLE KEYS */;
UNLOCK TABLES;
COMMIT;
SET AUTOCOMMIT=@OLD_AUTOCOMMIT;

--
-- Table structure for table `Softwares`
--

DROP TABLE IF EXISTS `Softwares`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `Softwares` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `NomeModulo` varchar(100) NOT NULL,
  `Versao` varchar(50) NOT NULL,
  `Licenca` varchar(50) DEFAULT NULL,
  `created_by` int DEFAULT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleted_at` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_softwares_createdby` (`created_by`),
  CONSTRAINT `fk_softwares_createdby` FOREIGN KEY (`created_by`) REFERENCES `Usuarios` (`Id`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Softwares`
--

SET @OLD_AUTOCOMMIT=@@AUTOCOMMIT, @@AUTOCOMMIT=0;
LOCK TABLES `Softwares` WRITE;
/*!40000 ALTER TABLE `Softwares` DISABLE KEYS */;
INSERT INTO `Softwares` VALUES
(1,'CUDA Toolkit','12.2','Proprietaria',NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(2,'OpenMPI','4.1.5','Open Source',NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(3,'GCC','13.2','GPL',NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(4,'Python','3.11','PSF',NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(5,'TensorFlow','2.15','Apache 2.0',NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(6,'PyTorch','2.1','BSD',NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(7,'Docker','24.0','Apache 2.0',NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL),
(8,'Slurm','23.02','GPL',NULL,'2026-06-16 01:00:24','2026-06-16 01:00:24',NULL);
/*!40000 ALTER TABLE `Softwares` ENABLE KEYS */;
UNLOCK TABLES;
COMMIT;
SET AUTOCOMMIT=@OLD_AUTOCOMMIT;

--
-- Table structure for table `Usuarios`
--

DROP TABLE IF EXISTS `Usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `Usuarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Login` varchar(50) NOT NULL,
  `Senha` varchar(50) NOT NULL,
  `PesquisadorId` int DEFAULT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  `updated_at` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleted_at` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Login` (`Login`),
  KEY `fk_usuarios_pesquisador` (`PesquisadorId`),
  CONSTRAINT `fk_usuarios_pesquisador` FOREIGN KEY (`PesquisadorId`) REFERENCES `Pesquisadores` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Usuarios`
--

SET @OLD_AUTOCOMMIT=@@AUTOCOMMIT, @@AUTOCOMMIT=0;
LOCK TABLES `Usuarios` WRITE;
/*!40000 ALTER TABLE `Usuarios` DISABLE KEYS */;
INSERT INTO `Usuarios` VALUES
(1,'admin@teste.com','123456',1,'2026-06-16 01:00:24','2026-06-16 02:25:07',NULL);
/*!40000 ALTER TABLE `Usuarios` ENABLE KEYS */;
UNLOCK TABLES;
COMMIT;
SET AUTOCOMMIT=@OLD_AUTOCOMMIT;

--
-- Dumping routines for database 'hpc_cluster'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*M!100616 SET NOTE_VERBOSITY=@OLD_NOTE_VERBOSITY */;

-- Dump completed on 2026-06-16  8:43:21
