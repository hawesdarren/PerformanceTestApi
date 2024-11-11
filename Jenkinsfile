
pipeline {
    agent{
	label 'jenkins-agent-ubuntu'
	}
	
	stages{
		stage('Git checkout') {
			steps{
				echo 'delete any prev project'
			    catchError(buildResult: 'SUCCESS') {
                    sh 'docker compose down --rmi local'
                    deleteDir()
                }
				echo 'check out'
				git branch: 'master', url: 'https://github.com/hawesdarren/PerformanceTestApi.git'
			}
			
		}
		stage('Deploy') {
			steps{
				echo 'Deploy'
				sh 'docker build --tag performance-test-api-image --no-cache --file Dockerfile .'
				//Docker image needs to be copied to microk8s
				sh 'docker save performance-test-api-image > performance-test-api-image.tar'
                sh 'microk8s ctr image import performance-test-api-image.tar'
                //Apply Deploy and Service
    			sh 'microk8s kubectl apply -f deployment.yaml'
    			sh 'microk8s kubectl apply -f service.yaml'	
		   
			}
			
		}
		stage('Test') {
			steps{
				echo 'run a test'
				sh 'docker compose up'
                //Archive test results
                sh 'mkdir artifacts'
                sh "docker container cp performance-test-api-test:app/IntergrationTests/TestResults/ ${env.WORKSPACE}/artifacts/test-results/"
                archiveArtifacts artifacts: 'artifacts/test-results/', followSymlinks: false
                //Clean up
                sh 'docker compose down --rmi local'
           
			}
			
		}
		stage('Produce test report'){
		    steps{
		        echo 'producing test report'
		        publishHTML([   allowMissing: true, 
		                        alwaysLinkToLastBuild: true, 
		                        keepAll: true, 
		                        reportDir: 'artifacts/test-results/', 
		                        reportFiles: 'TestResult*.html', 
		                        reportName: 'Test Report', 
		                        reportTitles: '', 
		                        useWrapperFileDirectly: true])
		        
		    }
		}
		stage('Performance test') {
			steps{
    			echo 'Performance test - todo'
    			dir('K6PerformanceTest'){
    			    sh 'docker compose up'
    			    archiveArtifacts allowEmptyArchive: true, artifacts: 'src/*.html, src/*.csv'
    			}

			}

		}
		stage('Produce performance test report') {
			steps{
    			echo 'Produce performance test report - todo'
    			//archiveArtifacts allowEmptyArchive: true, artifacts: 'K6PerformanceTest/src/*.html, K6PerformanceTest/src/*.csv'
    			//Publish performance test report
    			publishHTML([  allowMissing: false, 
		                        alwaysLinkToLastBuild: true, 
		                        keepAll: true, 
		                        reportDir: './', 
		                        reportFiles: 'K6PerformanceTest/src/*.html', 
		                        reportName: 'K6 Performance Test Report', 
		                        reportTitles: 'K6 Performance Test Report', 
		                        includes: '**/*.html,**/*.css',
		                        useWrapperFileDirectly: true])

			}

		}
	}
	
}