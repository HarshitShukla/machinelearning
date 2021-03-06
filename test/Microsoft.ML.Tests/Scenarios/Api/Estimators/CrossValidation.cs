﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.ML.Runtime.Data;
using Microsoft.ML.Runtime.RunTests;
using Microsoft.ML.Transforms.Categorical;
using Microsoft.ML.Transforms.Conversions;
using Xunit;
using System;
using System.Linq;

namespace Microsoft.ML.Tests.Scenarios.Api
{
    public partial class ApiScenariosTests
    {
        /// <summary>
        /// Cross-validation: Have a mechanism to do cross validation, that is, you come up with
        /// a data source (optionally with stratification column), come up with an instantiable transform
        /// and trainer pipeline, and it will handle (1) splitting up the data, (2) training the separate
        /// pipelines on in-fold data, (3) scoring on the out-fold data, (4) returning the set of
        /// evaluations and optionally trained pipes. (People always want metrics out of xfold,
        /// they sometimes want the actual models too.)
        /// </summary>
        [Fact]
        void New_CrossValidation()
        {
            var ml = new MLContext(seed: 1, conc: 1);

            var data = ml.Data.ReadFromTextFile<SentimentData>(GetDataPath(TestDatasets.Sentiment.trainFilename), hasHeader: true);

            // Pipeline.
            var pipeline = ml.Transforms.Text.FeaturizeText("SentimentText", "Features")
                    .Append(ml.BinaryClassification.Trainers.StochasticDualCoordinateAscent("Label", "Features", advancedSettings: (s) => { s.ConvergenceTolerance = 1f; s.NumThreads = 1; }));

            var cvResult = ml.BinaryClassification.CrossValidate(data, pipeline);
        }
    }
}
