using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlarmGroupTree
{
    public class AlarmGroupTreeParser
    {
        private List<string> _treeList;
        private List<AlarmGroupTree> _tree;
        private int _niveles;

        public AlarmGroupTreeParser(List<string> tree)
        {
            _treeList = tree;
            _niveles = 0;

            // arma arbol
            _tree = _ArmingTree();
        }

        // Arma arbol 
        private List<AlarmGroupTree> _ArmingTree()
        {
            List<AlarmGroupTree> retorno = new List<AlarmGroupTree>();
            int listSize = 0;
            AlarmGroupTree treeItem;


            // get list size
            listSize = _treeList.Count;

            foreach (string item in _treeList)
            {
                // split item
                string[] levelItems = item.Split('.');

                int i = 0;
                string father = "";

                foreach (string level in levelItems)
                {
                    // level
                    i++;
                    
                    // new tree item
                    treeItem = new AlarmGroupTree();

                    // tree item definition
                    treeItem.level = i;
                    treeItem.description = level;
                    treeItem.order = 0;
                    treeItem.father = father;

                    // add item to tree
                    retorno.Add(treeItem);

                    father = level;
                }

                if (i > _niveles)
                {
                    _niveles = i;
                }

                
            }

            return retorno;
        }

        public List<string> GetLevelList(int level)
        {
            List<string> retorno = new List<string>();

            
            return retorno;
        }
    }

    public class AlarmGroupTree
    {
        public string description;
        public string father;
        public int level;
        public int order;

        public AlarmGroupTree()
        {
        }


    }
}