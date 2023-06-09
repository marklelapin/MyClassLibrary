using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace LSMTests.LocalSQLDB
{
    [TestClass()]
    public class SqlServerUnitTest1 : SqlDatabaseTestClass
    {

        public SqlServerUnitTest1()
        {
            InitializeComponent();
        }

        [TestInitialize()]
        public void TestInitialize()
        {
            base.InitializeTest();
        }
        [TestCleanup()]
        public void TestCleanup()
        {
            base.CleanupTest();
        }

        #region Designer support code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spChangeToSyncedTest_TestAction;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlServerUnitTest1));
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spGetLocalLastSyncDateTest_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ChecksumCondition checksumCondition1;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spLoadChangesFromServerTest_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ChecksumCondition checksumCondition3;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spLoadFromLocalTest_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition4;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spSaveLocalStoragePathToServerTest_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition5;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spSaveToLocalAsChangeTest_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition6;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spSaveToServerTest_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition inconclusiveCondition7;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spSetLocalLastSyncDateTest_TestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ChecksumCondition CorrectValueInserted;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spGetLocalLastSyncDateTest_PretestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spLoadChangesFromServerTest_PretestAction;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ChecksumCondition checksumCondition2;
            Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction dbo_spLoadFromLocalTest_PretestAction;
            this.dbo_spChangeToSyncedTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.dbo_spGetLocalLastSyncDateTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.dbo_spLoadChangesFromServerTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.dbo_spLoadFromLocalTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.dbo_spSaveLocalStoragePathToServerTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.dbo_spSaveToLocalAsChangeTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.dbo_spSaveToServerTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            this.dbo_spSetLocalLastSyncDateTestData = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions();
            dbo_spChangeToSyncedTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            inconclusiveCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
            dbo_spGetLocalLastSyncDateTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            checksumCondition1 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ChecksumCondition();
            dbo_spLoadChangesFromServerTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            checksumCondition3 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ChecksumCondition();
            dbo_spLoadFromLocalTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            inconclusiveCondition4 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
            dbo_spSaveLocalStoragePathToServerTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            inconclusiveCondition5 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
            dbo_spSaveToLocalAsChangeTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            inconclusiveCondition6 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
            dbo_spSaveToServerTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            inconclusiveCondition7 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition();
            dbo_spSetLocalLastSyncDateTest_TestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            CorrectValueInserted = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ChecksumCondition();
            dbo_spGetLocalLastSyncDateTest_PretestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            dbo_spLoadChangesFromServerTest_PretestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            checksumCondition2 = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.ChecksumCondition();
            dbo_spLoadFromLocalTest_PretestAction = new Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction();
            // 
            // dbo_spChangeToSyncedTest_TestAction
            // 
            dbo_spChangeToSyncedTest_TestAction.Conditions.Add(inconclusiveCondition1);
            resources.ApplyResources(dbo_spChangeToSyncedTest_TestAction, "dbo_spChangeToSyncedTest_TestAction");
            // 
            // inconclusiveCondition1
            // 
            inconclusiveCondition1.Enabled = true;
            inconclusiveCondition1.Name = "inconclusiveCondition1";
            // 
            // dbo_spGetLocalLastSyncDateTest_TestAction
            // 
            dbo_spGetLocalLastSyncDateTest_TestAction.Conditions.Add(checksumCondition1);
            resources.ApplyResources(dbo_spGetLocalLastSyncDateTest_TestAction, "dbo_spGetLocalLastSyncDateTest_TestAction");
            // 
            // checksumCondition1
            // 
            checksumCondition1.Checksum = "-1836877491";
            checksumCondition1.Enabled = true;
            checksumCondition1.Name = "checksumCondition1";
            // 
            // dbo_spLoadChangesFromServerTest_TestAction
            // 
            dbo_spLoadChangesFromServerTest_TestAction.Conditions.Add(checksumCondition3);
            resources.ApplyResources(dbo_spLoadChangesFromServerTest_TestAction, "dbo_spLoadChangesFromServerTest_TestAction");
            // 
            // checksumCondition3
            // 
            checksumCondition3.Checksum = null;
            checksumCondition3.Enabled = true;
            checksumCondition3.Name = "checksumCondition3";
            // 
            // dbo_spLoadFromLocalTest_TestAction
            // 
            dbo_spLoadFromLocalTest_TestAction.Conditions.Add(inconclusiveCondition4);
            resources.ApplyResources(dbo_spLoadFromLocalTest_TestAction, "dbo_spLoadFromLocalTest_TestAction");
            // 
            // inconclusiveCondition4
            // 
            inconclusiveCondition4.Enabled = true;
            inconclusiveCondition4.Name = "inconclusiveCondition4";
            // 
            // dbo_spSaveLocalStoragePathToServerTest_TestAction
            // 
            dbo_spSaveLocalStoragePathToServerTest_TestAction.Conditions.Add(inconclusiveCondition5);
            resources.ApplyResources(dbo_spSaveLocalStoragePathToServerTest_TestAction, "dbo_spSaveLocalStoragePathToServerTest_TestAction");
            // 
            // inconclusiveCondition5
            // 
            inconclusiveCondition5.Enabled = true;
            inconclusiveCondition5.Name = "inconclusiveCondition5";
            // 
            // dbo_spSaveToLocalAsChangeTest_TestAction
            // 
            dbo_spSaveToLocalAsChangeTest_TestAction.Conditions.Add(inconclusiveCondition6);
            resources.ApplyResources(dbo_spSaveToLocalAsChangeTest_TestAction, "dbo_spSaveToLocalAsChangeTest_TestAction");
            // 
            // inconclusiveCondition6
            // 
            inconclusiveCondition6.Enabled = true;
            inconclusiveCondition6.Name = "inconclusiveCondition6";
            // 
            // dbo_spSaveToServerTest_TestAction
            // 
            dbo_spSaveToServerTest_TestAction.Conditions.Add(inconclusiveCondition7);
            resources.ApplyResources(dbo_spSaveToServerTest_TestAction, "dbo_spSaveToServerTest_TestAction");
            // 
            // inconclusiveCondition7
            // 
            inconclusiveCondition7.Enabled = true;
            inconclusiveCondition7.Name = "inconclusiveCondition7";
            // 
            // dbo_spSetLocalLastSyncDateTest_TestAction
            // 
            dbo_spSetLocalLastSyncDateTest_TestAction.Conditions.Add(CorrectValueInserted);
            resources.ApplyResources(dbo_spSetLocalLastSyncDateTest_TestAction, "dbo_spSetLocalLastSyncDateTest_TestAction");
            // 
            // CorrectValueInserted
            // 
            CorrectValueInserted.Checksum = "1027635817";
            CorrectValueInserted.Enabled = true;
            CorrectValueInserted.Name = "CorrectValueInserted";
            // 
            // dbo_spGetLocalLastSyncDateTest_PretestAction
            // 
            resources.ApplyResources(dbo_spGetLocalLastSyncDateTest_PretestAction, "dbo_spGetLocalLastSyncDateTest_PretestAction");
            // 
            // dbo_spLoadChangesFromServerTest_PretestAction
            // 
            dbo_spLoadChangesFromServerTest_PretestAction.Conditions.Add(checksumCondition2);
            resources.ApplyResources(dbo_spLoadChangesFromServerTest_PretestAction, "dbo_spLoadChangesFromServerTest_PretestAction");
            // 
            // checksumCondition2
            // 
            checksumCondition2.Checksum = null;
            checksumCondition2.Enabled = true;
            checksumCondition2.Name = "checksumCondition2";
            // 
            // dbo_spChangeToSyncedTestData
            // 
            this.dbo_spChangeToSyncedTestData.PosttestAction = null;
            this.dbo_spChangeToSyncedTestData.PretestAction = null;
            this.dbo_spChangeToSyncedTestData.TestAction = dbo_spChangeToSyncedTest_TestAction;
            // 
            // dbo_spGetLocalLastSyncDateTestData
            // 
            this.dbo_spGetLocalLastSyncDateTestData.PosttestAction = null;
            this.dbo_spGetLocalLastSyncDateTestData.PretestAction = dbo_spGetLocalLastSyncDateTest_PretestAction;
            this.dbo_spGetLocalLastSyncDateTestData.TestAction = dbo_spGetLocalLastSyncDateTest_TestAction;
            // 
            // dbo_spLoadChangesFromServerTestData
            // 
            this.dbo_spLoadChangesFromServerTestData.PosttestAction = null;
            this.dbo_spLoadChangesFromServerTestData.PretestAction = dbo_spLoadChangesFromServerTest_PretestAction;
            this.dbo_spLoadChangesFromServerTestData.TestAction = dbo_spLoadChangesFromServerTest_TestAction;
            // 
            // dbo_spLoadFromLocalTestData
            // 
            this.dbo_spLoadFromLocalTestData.PosttestAction = null;
            this.dbo_spLoadFromLocalTestData.PretestAction = dbo_spLoadFromLocalTest_PretestAction;
            this.dbo_spLoadFromLocalTestData.TestAction = dbo_spLoadFromLocalTest_TestAction;
            // 
            // dbo_spSaveLocalStoragePathToServerTestData
            // 
            this.dbo_spSaveLocalStoragePathToServerTestData.PosttestAction = null;
            this.dbo_spSaveLocalStoragePathToServerTestData.PretestAction = null;
            this.dbo_spSaveLocalStoragePathToServerTestData.TestAction = dbo_spSaveLocalStoragePathToServerTest_TestAction;
            // 
            // dbo_spSaveToLocalAsChangeTestData
            // 
            this.dbo_spSaveToLocalAsChangeTestData.PosttestAction = null;
            this.dbo_spSaveToLocalAsChangeTestData.PretestAction = null;
            this.dbo_spSaveToLocalAsChangeTestData.TestAction = dbo_spSaveToLocalAsChangeTest_TestAction;
            // 
            // dbo_spSaveToServerTestData
            // 
            this.dbo_spSaveToServerTestData.PosttestAction = null;
            this.dbo_spSaveToServerTestData.PretestAction = null;
            this.dbo_spSaveToServerTestData.TestAction = dbo_spSaveToServerTest_TestAction;
            // 
            // dbo_spSetLocalLastSyncDateTestData
            // 
            this.dbo_spSetLocalLastSyncDateTestData.PosttestAction = null;
            this.dbo_spSetLocalLastSyncDateTestData.PretestAction = null;
            this.dbo_spSetLocalLastSyncDateTestData.TestAction = dbo_spSetLocalLastSyncDateTest_TestAction;
            // 
            // dbo_spLoadFromLocalTest_PretestAction
            // 
            resources.ApplyResources(dbo_spLoadFromLocalTest_PretestAction, "dbo_spLoadFromLocalTest_PretestAction");
        }

        #endregion


        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        #endregion

        [TestMethod()]
        public void dbo_spChangeToSyncedTest()
        {
            SqlDatabaseTestActions testActions = this.dbo_spChangeToSyncedTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }

        [TestMethod()]
        public void dbo_spGetLocalLastSyncDateTest()
        {
            SqlDatabaseTestActions testActions = this.dbo_spGetLocalLastSyncDateTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }

        [TestMethod()]
        public void dbo_spLoadChangesFromServerTest()
        {
            SqlDatabaseTestActions testActions = this.dbo_spLoadChangesFromServerTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }

        [TestMethod()]
        public void dbo_spLoadFromLocalTest()
        {
            SqlDatabaseTestActions testActions = this.dbo_spLoadFromLocalTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }

        [TestMethod()]
        public void dbo_spSaveLocalStoragePathToServerTest()
        {
            SqlDatabaseTestActions testActions = this.dbo_spSaveLocalStoragePathToServerTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }

        [TestMethod()]
        public void dbo_spSaveToLocalAsChangeTest()
        {
            SqlDatabaseTestActions testActions = this.dbo_spSaveToLocalAsChangeTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }

        [TestMethod()]
        public void dbo_spSaveToServerTest()
        {
            SqlDatabaseTestActions testActions = this.dbo_spSaveToServerTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }

        [TestMethod()]
        public void dbo_spSetLocalLastSyncDateTest()
        {
            SqlDatabaseTestActions testActions = this.dbo_spSetLocalLastSyncDateTestData;
            // Execute the pre-test script
            // 
            System.Diagnostics.Trace.WriteLineIf((testActions.PretestAction != null), "Executing pre-test script...");
            SqlExecutionResult[] pretestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PretestAction);
            try
            {
                // Execute the test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.TestAction != null), "Executing test script...");
                SqlExecutionResult[] testResults = TestService.Execute(this.ExecutionContext, this.PrivilegedContext, testActions.TestAction);
            }
            finally
            {
                // Execute the post-test script
                // 
                System.Diagnostics.Trace.WriteLineIf((testActions.PosttestAction != null), "Executing post-test script...");
                SqlExecutionResult[] posttestResults = TestService.Execute(this.PrivilegedContext, this.PrivilegedContext, testActions.PosttestAction);
            }
        }
        private SqlDatabaseTestActions dbo_spChangeToSyncedTestData;
        private SqlDatabaseTestActions dbo_spGetLocalLastSyncDateTestData;
        private SqlDatabaseTestActions dbo_spLoadChangesFromServerTestData;
        private SqlDatabaseTestActions dbo_spLoadFromLocalTestData;
        private SqlDatabaseTestActions dbo_spSaveLocalStoragePathToServerTestData;
        private SqlDatabaseTestActions dbo_spSaveToLocalAsChangeTestData;
        private SqlDatabaseTestActions dbo_spSaveToServerTestData;
        private SqlDatabaseTestActions dbo_spSetLocalLastSyncDateTestData;
    }
}
