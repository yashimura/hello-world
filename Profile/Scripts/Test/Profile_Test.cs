////
// Created by Eroshenko Sergei [ESA/Ixagami] [http://esa.pw]
// sergeie@cosmomachia.co.jp
// owneresa@gmail.com
// 26.10.2018 15:31:02
////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mix2App.Lib.Events;
using Mix2App.UI;
using Mix2App.Lib.Model;

namespace Mix2App.Profile.Test {
    public class Profile_Test: MonoBehaviour {
        [SerializeField] private UserKind UKind = 0;
        [SerializeField] private UserType UType = 0;

        [SerializeField] private MonoBehaviour TestedCore = null;

        [SerializeField] private bool Ch1Enabled = true;
        [SerializeField] private bool Ch2Enabled = false;

        private void Start() {
            List<int> backbodylist = new List<int> { 16, 17, 18, 21, 22, 23, 24, 27, 31, 32, 34, 35, 36, 37, 39, 40, 42, 43, 44 };

            // Fill family tree
            List<FamilyTree> ft_list = new List<FamilyTree>();
            for (int i = 0; i < 20; i++) {
                if (Random.Range(0, 2) == 0) {
                    ft_list.Add(new FamilyTree() {
                        chara1 = new TamaChara(backbodylist[Random.Range(0, backbodylist.Count)]),
                        chara2 = new TamaChara(backbodylist[Random.Range(0, backbodylist.Count)]),
                        chara3 = null
                    });
                } else
                    ft_list.Add(new FamilyTree() {
                        chara1 = new TamaChara(backbodylist[Random.Range(0, backbodylist.Count)]),
                        chara3 = new TamaChara(backbodylist[Random.Range(0, backbodylist.Count)]),
                        chara2 = null
                    });
            }

            int count = Random.Range(3, 8);

            User usr = new User(Random.Range(1, int.MaxValue)) {
                ftrees = ft_list,
                pet = new TamaPet(Random.Range(700, 710)),
                avatar = new TamaAvatar(),
                calbums = new List<AlbumData>(),
                chara1 = (Ch1Enabled) ? new TamaChara(backbodylist[Random.Range(0, backbodylist.Count)]) : null,
                chara2 = (Ch2Enabled) ? (new TamaChara(backbodylist[Random.Range(0, backbodylist.Count)])) : null,
                friends1 = new List<User>(),
                bplace = Random.Range(0, 48),
                ukind = UKind,
                utype = UType
            };

            if (usr.chara1 != null)
                usr.chara1.wstyle = "あいうえお";


            for (int i = 0; i < count; i++)
                usr.calbums.Add(new AlbumData(16));

            count = Random.Range(3, 40);

            for (int i = 0; i < count; i++)
                usr.friends1.Add(new User(0) {
                    chara1 = new TamaChara(backbodylist[Random.Range(0, backbodylist.Count)]),
                    pet = new TamaPet(Random.Range(700, 710)),
                });

            //usr = new TestUser(901, UserKind.ANOTHER, UserType.MIX2, 16, 0, 1, 1);
            IReceiver TestedScene = TestedCore as IReceiver;
            TestedScene.receive(usr);

        }
    }
}
