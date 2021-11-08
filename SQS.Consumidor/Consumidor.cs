﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Data;
using System.Text.Json;

namespace SQS.Consumidor
{
    class Consumidor
    {
        static async Task Main(string[] args)
        {
            string queeueUrl = "https://sqs.us-east-1.amazonaws.com/852704159394/drcash-logs-api-test";
            
            var client = new AmazonSQSClient(RegionEndpoint.USEast1);
            var request = new ReceiveMessageRequest
            {
                QueueUrl = queeueUrl,
                WaitTimeSeconds = 15
            };
            
            var response = await client.ReceiveMessageAsync(request);
            var user = new Users();
            
            foreach (var message in response.Messages)
            { 
                user = JsonSerializer.Deserialize<Users>(message.Body);
                await client.DeleteMessageAsync(queeueUrl, message.ReceiptHandle);
            }
            
            VerifyIfThereIsRecivedMessage(user, response.Messages.Any());
        }

        private static void VerifyIfThereIsRecivedMessage(Users user, bool hasMessage)
        {
            if (hasMessage)
            { 
                Console.WriteLine($"Recived user {user.Nome} with email: {user.Email}"); 
                Console.WriteLine("Mensagem recebida com sucesso!!!");
            }
        }
    }
}